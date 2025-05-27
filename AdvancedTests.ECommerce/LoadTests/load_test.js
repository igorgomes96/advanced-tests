import http from 'k6/http';
import { sleep, check } from 'k6';
import exec from 'k6/execution';
import { SharedArray } from 'k6/data';
import { generateRandomOrders } from './orders_generator.js';

const BASE_URL = 'http://localhost:5000';

const customerIds = new SharedArray('customerIds', () => [...Array(1000).keys()].map(x => x + 1));
const orders = new SharedArray('orders', () => generateRandomOrders(2000));

export const options = {
    thresholds: {
        http_req_failed: ['rate < 0.01'],
        http_req_duration: ['avg < 20', 'med < 12', 'min < 5', 'p(90) < 25', 'p(95) < 30', 'p(99.9) < 40'],
        checks: ['rate > 0.9']
    },
    scenarios: {
        sharedIterations: {
            executor: 'shared-iterations',
            exec: 'getOrder',
            vus: 10,
            iterations: 50,
            maxDuration: '15s'
        },
        perVuIterations: {
            executor: 'per-vu-iterations',
            exec: 'createOrder',
            vus: 10,
            iterations: 10,
            maxDuration: '25s'
        },
        constantVus: {
            executor: 'constant-vus',
            exec: 'getOrder',
            vus: 10,
            duration: '10s',
            startTime: '5s'
        },
        rampingVus: {
            executor: 'ramping-vus',
            exec: 'getOrder',
            startTime: '10s',
            stages: [
                { duration: '5s', target: 50 },
                { duration: '10s', target: 50 },
                { duration: '5s', target: 0 },
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
        'is status 200': (r) => r.status === 200,
        'result is not empty list': (r) => r.body.length > 2
    });
    sleep(1);
}

export function createOrder(data) {
    const params = {
        headers: {
            'Authorization': 'Bearer ' + data.token,
            'Content-Type': 'application/json'
        },
    };
    const payload = orders[exec.vu.iterationInInstance % orders.length];
    const res = http.post(BASE_URL + `/orders`, JSON.stringify(payload), params);
    check(res, {
        'is status 200': (r) => r.status === 200,
        'returns id': (r) => r.json().id
    });
    sleep(1);
}

export function teardown(data) {
    console.log('TearDown: ', data);
}