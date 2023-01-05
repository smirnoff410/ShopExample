import React from 'react';
import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import ProductCard from './ProductCard';
import '../../App.css';

const API_URL = 'http://localhost:5000/api/catalog';

const Product = () => {
    const [products, setProducts] = useState([]);

    const getCatalog = async () => {
        const response = await fetch(`${API_URL + '/product'}`);
        const data = await response.json();
        setProducts(data)
    }

    useEffect(() => {
        getCatalog()
    }, [])

    return (
        <div className="app">
            <div className="navigation">
                <h1 className="title">ShopExample app</h1>
                <div>
                <Link className="basketLink" to="/basket">Basket</Link>
                <Link className="usersLink" to="/users">Users</Link>
                </div>
            </div>
            
            {
                products?.length > 0 ? (
                    <div className="content">
                        {products.map((item) => (
                            <ProductCard product={item}/>
                        ))}
                        
                    </div>
                ) :
                (
                    <div className="empty">
                        <h2>No products found</h2>
                    </div>
                )
            }
        </div>
    )
}

export default Product;