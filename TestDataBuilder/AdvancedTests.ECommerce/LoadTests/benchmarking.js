import http from 'k6/http';
import { sleep, check } from 'k6';
import exec from 'k6/execution';
import { SharedArray } from 'k6/data';

const BASE_URL = 'https://localhost:5001';

const customerIds = new SharedArray('customerIds', () => [...Array(250).keys()].map(x => x + 1));

// 100 VUs
// 30.000 iteracões
// http1 vs http2
// warm up de 10s

export const options = {
    scenarios: {
        warmUpHttp1: {
            executor: 'constant-vus',
            exec: 'http1',
            vus: 100,
            duration: '10s'
        },
        http1: {
            executor: 'shared-iterations',
            exec: 'http1',
            vus: 100,
            iterations: 30000,
            startTime: '10s'
        },
        http1Compressed: {
            executor: 'shared-iterations',
            exec: 'http1Compressed',
            vus: 100,
            iterations: 30000,
            startTime: '2m'
        },
        warmUpHttp2: {
            executor: 'constant-vus',
            exec: 'http2',
            vus: 100,
            duration: '10s',
            startTime: '4m'
        },
        http2: {
            executor: 'shared-iterations',
            exec: 'http2',
            vus: 100,
            iterations: 30000,
            startTime: '4m10s'
        },
        http2Compressed: {
            executor: 'shared-iterations',
            exec: 'http2Compressed',
            vus: 100,
            iterations: 30000,
            startTime: '6m10s'
        }
    }
};

export function setup() {
    return { token: 'jwt-token' };
}

export function http1(data) {
    const params = {
        headers: {
            'Authorization': 'Bearer ' + data.token,
        },
        httpProtocol: 'HTTP/1.1'
    };
    getOrder(params);
    sleep(0.1);
}


export function http1Compressed(data) {
    const params = {
        headers: {
            'Authorization': 'Bearer ' + data.token,
            'Accept-Encoding': 'gzip'
        },
        httpProtocol: 'HTTP/1.1'
    };
    getOrder(params);
    sleep(0.1);
}

export function http2(data) {
    const params = {
        headers: {
            'Authorization': 'Bearer ' + data.token,
        },
        httpProtocol: 'HTTP/2'
    };
    getOrder(params);
    sleep(0.1);
}

export function http2Compressed(data) {
    const params = {
        headers: {
            'Authorization': 'Bearer ' + data.token,
            'Accept-Encoding': 'gzip'
        },
        httpProtocol: 'HTTP/2'
    };
    getOrder(params);
    sleep(0.1);
}

function getOrder(params) {
    const id = customerIds[exec.vu.iterationInInstance % customerIds.length];
    const res = http.get(BASE_URL + `/orders?customerId=${id}`, params);
    check(res, {
        'is status 200': (r) => r.status === 200,
        'result is not empty list': (r) => r.body.length > 2
    });
    sleep(0.1);
}