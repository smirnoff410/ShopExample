import React from 'react';
import {
    BrowserRouter as Router,
    Routes,
    Switch,
    Route,
    Link
  } from "react-router-dom";
import Basket from './pages/Basket/Basket';
import Product from './pages/Product/Product';
import User from './pages/User/User';

const App = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Product/>}></Route>
                <Route path="/basket" element={<Basket/>}></Route>
                <Route path="/users" element={<User/>}></Route>
            </Routes>
        </Router>
    );
}

export default App;