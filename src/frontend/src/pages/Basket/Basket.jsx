import React from 'react';
import { useState, useEffect } from 'react';
import '../../App.css';
import { Link } from 'react-router-dom';
import BasketCard from './BasketCard';

const API_URL = 'api/basket';

const Basket = () => {
    const [basket, setBasket] = useState([]);

    const getBasket = async () => {
        var userId = localStorage.getItem('user-id')
        const response = await fetch(`${API_URL + '/basket/' + userId}`);
        const data = await response.json();
        console.log(data)
        setBasket(data)
    }

    useEffect(() => {
        getBasket()
    }, [])
    
    return(
        <div className="basket-card">
            <div className="basket-header">
                <h3 className="basket-heading">Shopping Cart</h3>
                <Link className="basket-action" to="/">Return to shop</Link>
            </div>
            {
                basket?.products?.length > 0 ? (
                    <div>
                    <div className="content">
                        {basket.products.map((item) => (
                            <BasketCard basketItem={item}/>
                        ))}
                        
                    </div>
                    <hr/> 
                    <div className="checkout">
                        <div class="total">
                        <div>
                            <div className="Subtotal">Sub-Total</div>
                        </div>
                            <div className="total-amount">{basket.totalPrice} $</div>
                        </div>
                        <button className="basket-button" onClick={async() => {
                            const userId = localStorage.getItem('user-id')
                            if(userId == null){
                                alert('Выберите пользователя')
                                return;
                            }
                            const requestOptions = {
                                method: 'POST',
                                headers: { 'Content-Type': 'application/json' }
                            }
                            const order = await fetch(`${API_URL}/basket/${userId}/order`, requestOptions)
                            if(order.status == 400){
                                const data = await order.text()
                                alert(data)
                            }
                            else{
                                alert('Заказ успешно создан')
                            }
                        }}>Checkout</button>
                    </div>
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

export default Basket;