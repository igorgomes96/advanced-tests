export function generateRandomOrders(quantity = 1000) {
    const orders = [];

    const randomString = (length) =>
        Math.random().toString(36).substring(2, 2 + length);

    const randomStateCode = () =>
        String.fromCharCode(65 + Math.floor(Math.random() * 26)) +
        String.fromCharCode(65 + Math.floor(Math.random() * 26));

    const randomZipCode = () =>
        `${Math.floor(10000 + Math.random() * 90000)}-${Math.floor(100 + Math.random() * 900)}`;

    for (let i = 0; i < quantity; i++) {
        const order = {
            customerId: Math.floor(Math.random() * 1000) + 1,
            address: {
                street: randomString(10),
                city: randomString(8),
                state: randomStateCode(),
                zipCode: randomZipCode(),
                number: Math.floor(Math.random() * 1000) + 1
            },
            items: Array.from({ length: Math.floor(Math.random() * 5) + 1 }).map(() => ({
                name: randomString(6),
                quantity: Math.floor(Math.random() * 10) + 1,
                price: parseFloat((Math.random() * 100).toFixed(2))
            }))
        };

        orders.push(order);
    }

    return orders;
}
