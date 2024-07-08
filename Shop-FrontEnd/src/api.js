let fakeProducts = [
  { id: 1, name: 'Футболка', price: 20 },
  { id: 2, name: 'Штаны', price: 50 },
  { id: 3, name: 'Аксессуар', price: 10 },
];

export const getProducts = async () => {
  // Здесь можно вернуть фиктивные данные
  return fakeProducts;
};

export const addProduct = async (product) => {
  // Здесь можно вернуть фиктивный успешный ответ
  const addedProduct = { ...product, id: fakeProducts.length + 1 };
  fakeProducts.push(addedProduct);
  return addedProduct;
};