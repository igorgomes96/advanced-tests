import http from 'k6/http';
import { sleep } from 'k6';

const BASE_URL = 'http://localhost:5000';

export const options = {
    // iterations: 10,
    vus: 5,
    duration: '20s'
};

export default () => {
    http.get(BASE_URL + '/orders?customerId=1');

    sleep(1);
}