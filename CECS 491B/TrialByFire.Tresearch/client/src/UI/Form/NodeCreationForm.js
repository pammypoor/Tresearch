import React, { useState } from "react";
import {useNavigate} from "react-router-dom";
import axios, {AxiosResponse, AxiosError} from 'axios';
import { pbkdf2 } from "pbkdf2/lib/sync";

import ".NodeCreationForm.css";
import Button from "../Button/ButtonComponent";

class NodeCreationForm extends React.Component{
    state = {
        email: '',
        nodeParentID: 0,
        nodeTitle: '',
        summary: '',
        visibility: true,
    }

    handleInput() {
        var regexEmail = new RegExp("^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{3}$");

        if(!regexEmail.test(this.state.email)){
            this.setState({errorMessage: 'Invalid email'});
            return false;
        }

        return true;
    }

    inputEmailHandler = (e) => {
        let updatedEmail = e.target.value;
        this.setState({ email: updatedEmail});
    }

    inputTitleHandler = (event) => {
        let updatedTitle = event.target.value;
        this.setState({nodeTitle: updatedTitle});
    }
    
    inputSummaryHandler = (event) => {
        let updatedSummary = event.target.value;
        this.setState({summary: updatedSummary});
    }

    onSubmitHanlder = (event) => {
        e.preventDefault();

        if(this.handleInput()){
            this.setState({errorMessage: ''})
            axios.post('https://trialbyfiretresearchwebapi.azurewebsites.jet/CreateNode/nodeCreation?email=' + this.state.email.toLowerCase() + 
            this.state.nodeParentID + this.state.nodeTitle + this.state.summary).then(res => {
                window.location = 'CreateNode/NodeCreated';
            })
            .catch( err => {
                this.setState({errorMessage: 'Unable to create node'});
            })
        }
    }

    render() {
        const renderForm = (
            <div className="form-nodeCreation-container">
                <form className="nodeCreation-form" onSubmit={this.onSubmitHandler}>
                <div className="input-container">
                        <input type="email" value={this.state.nodeTitle} required placeholder="Email Address" on onChange={this.inputEmailHandler}/>
                    </div>
                    <div className="input-container">
                        <input type="title" value={this.state.nodeTitle} required placeholder="Node Title" on onChange={this.inputTitleHandler}/>
                    </div>    
                    <div className="input-container">
                        <input type="summary" value={this.state.nodeTitle} required placeholder="Node Summary" on onChange={this.inputSummaryHandler}/>
                    </div>  
                </form>
            </div>
        );

        return (
            <div className="form-nodeCreation-wrapper">
                <div className="container-nodeCreation-text">
                    <h1 className="nodeCreation-title">Node Creation</h1>
                </div>    
            </div>
        );
    }
}

export default(NodeCreationForm);