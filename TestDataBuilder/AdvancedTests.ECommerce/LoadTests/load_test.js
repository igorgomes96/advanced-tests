import http from 'k6/http';
import { sleep, check } from 'k6';
import exec from 'k6/execution';
import { SharedArray } from 'k6/data';

const BASE_URL = 'http://localhost:5000';

const payload = new SharedArray('payload', () => [...Array(1000).keys()].map(x => x + 1));

export const options = {
    stages: [
        { duration: '30s', target: 100 },  // Ramp-up: 0 → 100 VUs em 30s
        { duration: '1m', target: 100 },   // Carga sustentada
        { duration: '30s', target: 200 },  // Aumento da acesso
        { duration: '1m', target: 200 },   // Estabilização
        { duration: '1m', target: 0 },     // Ramp-down
    ]
};

export function setup() {
    return { token: 'jwt-token' };
}

export default (data) => {
    const params = {
        headers: {
            'Authorization': 'Bearer ' + data.token,
        },
    };
    const id = payload[exec.vu.iterationInInstance % payload.length];
    console.log(id);
    const res = http.get(BASE_URL + `/orders?customerId=${id}`, params);
    check(res, {
        'is status 200': (r) => r.status === 200,
        'result is not empty list': (r) => r.body.length > 2
    });
    sleep(1);
}

export function teardown(data) {
    console.log('TearDown: ', data);
}