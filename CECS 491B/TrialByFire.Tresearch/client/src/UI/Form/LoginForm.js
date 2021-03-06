import React from "react";
import axios from 'axios';
import pbkdf2 from "pbkdf2/lib/sync";
import "./LoginForm.css";
import Button from "../Button/ButtonComponent";

class LoginForm extends React.Component  {
    
    state = {
        email: '',
        passphrase: '',
        otp: '',
        verified: false,
        token: sessionStorage.getItem('authorization'),
        errorMessage: ''
    }
    
    handleInput() {
        // [username]@[domain name].[domain]
        // Username: a-z, A-Z, 0-9, .-
        // Domain Name: a-z, A-Z, 0-9, .-
        // Domain: a-z, A-Z
        // Satisfy all three requirements
        var regexUsername = new RegExp("^[a-zA-Z0-9.-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{3}$");

        //  passphrase: a-z, A-Z, 0-9, .,@! space
        var regexPassphrase = new RegExp("^[a-zA-Z0-9.,@!\s]+$");

        //  passphrase: a-z, A-Z, 0-9, .,@! space
        var regexOTP = new RegExp("^[a-zA-Z0-9]+$");
        
        if(this.state.verified){
            if(this.state.otp.length < 8){
                this.setState({errorMessage: 'OTP must be at least 8 characters'});
                return false;
            }
        }  
        else{
            if(this.state.passphrase.length < 8){
                this.setState({errorMessage: 'Password must be at least 8 characters'});
                return false;
            }
        }

        if(!regexUsername.test(this.state.username)){
            this.setState({errorMessage: 'Invalid username'});
            return false;
        }

        if(this.state.verified){
            if(!regexOTP.test(this.state.otp)){
                this.setState({errorMessage: 'Invalid otp'});
                return false;
            }
        }else{
            if(!regexPassphrase.test(this.state.passphrase)){
                this.setState({errorMessage: 'Invalid passphrase'});
                return false;
            }
        }
        return true; 
    }

    inputUsernameHandler = (e) => {
        let updatedUsername = e.target.value;
        this.setState({ username: updatedUsername});
    }

    inputPassphraseHandler = (e) => {
        let updatedPassphrase = e.target.value;
        this.setState({ passphrase: updatedPassphrase});
    }

    inputOTPHandler = (e) => {
        let updatedOTP = e.target.value;
        this.setState({ otp: updatedOTP});
    }

    hashInput = (value) => {
        var pbkdf2 = require('pbkdf2');      
        const pbkdfKey = pbkdf2.pbkdf2Sync(value, '',  10000,  64, 'sha512');
        return pbkdfKey.toString('hex').toUpperCase();
    }

    checkToken = () => {

    }

    onSubmitHandler = (e) => {
        e.preventDefault();
        this.checkToken();
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
        // pbkdf2 uses callbacks not promises, need to wrap in a promise object

        if(this.handleInput()){
            this.setState({errorMessage: ''})
            {this.state.verified ? 
                axios.post('https://trialbyfiretresearchwebapi.azurewebsites.net/Authentication/authenticate?username=' + this.state.username.toLowerCase() + 
                '&otp=' + this.hashInput(this.state.otp) + '&authorizationLevel=user')
                .then(response => {
                        sessionStorage.setItem('authorization', response.headers['authorization']);
                        window.location = '/Portal';
                }).catch(err => {
                        //sessionStorage.setItem('authorization', err.headers['authorization']);
                    })
                :
                axios.post('https://trialbyfiretresearchwebapi.azurewebsites.net/OTPRequest/requestotp?username=' + this.state.username.toLowerCase() + 
                '&passphrase=' + this.hashInput(this.state.passphrase) + '&authorizationLevel=user')
                .then(response => {
                        this.setState({verified: true});
                        
                        //navigate('/Login/Authentication');
                }).catch(err => {
                    switch(err.response.status){
                        case 400: this.setState({errorMessage: 'Invalid Username or Passphrase'});
                            break;
                        case 401: this.setState({errorMessage: 'Account is disabled'});
                            break;
                        default: this.setState({errorMessage: 'Unable to login'});
                    }
                })
            }
        }
        
        this.setState({ username: ''});
        this.setState({ passphrase: ''});
        this.setState({ otp: ''});
    }

    render() {
        const renderForm = (
            <div className="form-login-container">
                 <form className="login-form" onSubmit = {this.onSubmitHandler}>
                        <div className="input-container">
                            <input type="text" value={this.state.username} required placeholder="Username" onChange = {this.inputUsernameHandler}/>
                        </div>
                        <div className="input-container">
                            {this.state.verified ? 
                            (<input type="password" value={this.state.otp} required placeholder="OTP" onChange = {this.inputOTPHandler}/>)
                            :
                            (<input type="password" value={this.state.passphrase} required placeholder="Passphrase" onChange = {this.inputPassphraseHandler}/>)
                            }
                        </div>
                        <div className="create-button-container">
                            <Button type="button" color="green" name={this.state.verified ? "Login" : "Verify"}/>
                        </div>
                        {this.state.errorMessage && <div className="error-login"> {this.state.errorMessage} </div>}
                    </form>
            </div>
        );
        return (
            <div className="form-login-wrapper">
                <div className="container-login-text">
                    <h1 className="login-title">{this.state.verified ? "Login" : "Verify"}</h1>
                </div>
                {renderForm}
            </div>
        );
    }
}

export default(LoginForm);