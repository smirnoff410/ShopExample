import React from 'react';

const API_URL = 'http://localhost:5002/api';

const ProductCard = ({product}) => {
    return(
        <div className='container'>
            <div class="card">
                <div class="imgBx"> 
                    <img src={product.imageUrl}/>
                </div>
                <div class="contentBx">
                <h2>{product.name}</h2>
                <div class="description">
                    <h3>{product.description}</h3>
                    <h3>Price: {product.price}$</h3>
                </div>
                <a href="#" onClick={async () => {
                    const requestOptions = {
                        method: 'PUT',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ product: { id: product.id, count: 1} })
                    }
                    const userId = localStorage.getItem('user-id')
                    if(userId == null){
                        alert("Select user")
                        return;
                    }
                    await fetch(`${API_URL + '/basket/' + userId}`, requestOptions)
                    console.log(product.id)} }>Add to basket</a>
                </div>
            </div>
        </div>
    )
}

export default ProductCard;