import React from 'react';
import { useState, useEffect } from 'react';
import '../../App.css';

const BasketCard = ({basketItem}) => {
    const [counter, setCounter] = useState(basketItem.count);

    return(
        <div className="basket-cart-items">
            <div className="basket-image-box">
                <img style={{height: "120px"}} src={basketItem.imageUrl} />
            </div>
            <div className="basket-about">
            <h1 className="basket-title">{basketItem.name}</h1>
            </div>
            <div className="counter">
                <div className="btn" onClick={() => setCounter((prevCount) => prevCount - 1)}>-</div>
                <div className="count">{counter}</div>
                <div className="btn" onClick={() => setCounter((prevCount) => prevCount + 1)}>+</div>
            </div>
            <div className="prices">
                <div className="amount">{basketItem.price}$</div>
                <div className="save"><u>Save for later</u></div>
                <div className="remove"><u>Remove</u></div>
            </div>
        </div>
    )
}

export default BasketCard;