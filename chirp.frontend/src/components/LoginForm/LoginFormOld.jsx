import React, { useState, useEffect } from 'react';
import React from "react";
import * as Input from '../../../node_modules/postcss/lib/input';

export default class LoginForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { username: "", password: "" };
    }

    handleChangeUserName = (event) => {
        this.setState({ username: event.target.value });
    };

    handleChangePwd = (event) => {
        this.setState({ password: event.target.value });
    };

    handleSubmit = (event) => {
        event.preventDefault();
        console.log("username", this.state.username);
        console.log("password", this.state.password);
    };

    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <input
                    placeholder="Username"
                    type="text"
                    id="username"
                    value={this.state.username}
                    onChange={this.handleChangeUserName}
                />

                <input
                    placeholder="Password"
                    type="password"
                    id="password"
                    value={this.state.password}
                    onChange={this.handleChangePwd}
                />
                <input type="submit" value="submit" />
            </form>
        );
    }
}
    //return (
    //    <div>
    //        <p>Login</p>
    //        <div class="container">
    //            <label for="uname"><b>Username</b></label>
    //            <input type="text" placeholder="Username" name="uname" required></input>

    //            <label for="psw"><b>Password</b></label>
    //            <input type="password" placeholder="Password" name="psw" required></input>

    //            <button type="submit">Login</button>
    //            <label>
    //                <input type="checkbox" checked="checked" name="remember">Remember me</input>
    //            </label>
    //        </div>
    //    </div>
    //);