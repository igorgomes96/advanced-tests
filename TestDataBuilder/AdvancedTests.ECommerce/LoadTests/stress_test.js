import http from 'k6/http';
import { sleep, check } from 'k6';
import exec from 'k6/execution';
import { SharedArray } from 'k6/data';

const BASE_URL = 'http://localhost:5000';

const customerIds = new SharedArray('customerIds', () => [...Array(1000).keys()].map(x => x + 1));

export const options = {
    thresholds: {
        http_req_failed: ['rate < 0.1'],
        checks: ['rate > 0.9'],
        http_reqs: ['count > 50000'],
        http_req_duration: ['p(90) < 40']
    },
    scenarios: {
        getOrder: {
            executor: 'ramping-vus',
            exec: 'getOrder',
            stages: [
                { duration: '1m', target: 50 },   // Warm up
                { duration: '3m', target: 500 },
                { duration: '1m', target: 350 },
                { duration: '1m', target: 350 },  // Recuperação
                { duration: '3m', target: 0 },
            ]
        }
    }
};

export function setup() {
    return { token: 'jwt-token' };
}

export function getOrder(data) {
    const params = {
        headers: {
            'Authorization': 'Bearer ' + data.token,
        },
    };
    const id = customerIds[exec.vu.iterationInInstance % customerIds.length];
    const res = http.get(BASE_URL + `/orders?customerId=${id}`, params);
    check(res, {
        'is status 200': (r) => r?.status === 200,
        'result is not empty list': (r) => r?.body && r.body.length > 2
    });
    sleep(1);
}

export function teardown(data) {
    console.log('TearDown: ', data);
}