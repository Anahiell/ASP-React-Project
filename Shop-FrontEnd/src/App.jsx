import React, { useState, useEffect } from 'react';
import { Routes, Route } from 'react-router-dom';
import { getProducts, addProduct } from './api';
import Layout from './components/Layout';
import AboutPage from './components/AboutPage';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  const [products, setProducts] = useState([]);
  const [newProduct, setNewProduct] = useState({ name: '', price: 0 });

  useEffect(() => {
    getProducts().then(data => setProducts(data));
  }, []);

  const handleAddProduct = async () => {
    const addedProduct = await addProduct(newProduct);
    setProducts([...products, addedProduct]);
  };

  return (
      <Layout>
        <Routes>
          <Route path="/about" element={<AboutPage />} />
          <Route path="/" element={
            <div>
              <h1>Products</h1>
              <ul>
                {products.map(product => (
                  <li key={product.id}>{product.name}: ${product.price}</li>
                ))}
              </ul>
              <div>
                <h2>Add Product</h2>
                <input
                  type="text"
                  value={newProduct.name}
                  onChange={e => setNewProduct({ ...newProduct, name: e.target.value })}
                  placeholder="Product Name"
                />
                <input
                  type="number"
                  value={newProduct.price}
                  onChange={e => setNewProduct({ ...newProduct, price: parseFloat(e.target.value) })}
                  placeholder="Product Price"
                />
                <button onClick={handleAddProduct}>Add</button>
              </div>
            </div>
          } />
        </Routes>
      </Layout>
  );
}

export default App;
