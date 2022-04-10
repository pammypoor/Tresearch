import axios from "axios";
import React, {useState, useEffect } from "react";
import AuthenticatedNavBar from "../../UI/Navigation/AuthenticatedNavBar";


import "./TagDashboard.css";
import Button from "../../UI/Button/ButtonComponent";

function TagDashboard() {
    const [data, setData] = useState([]);

    const [createData, setCreateData] = useState('');
    

    const [alertData, setAlertData] = useState(
        {
            message: ''
        }
    )

    //Axios doesn't allow certain special characters. This will encode
    const  handleEncoded = (e) => {
        var parsedData = e.toString();
        if(parsedData.includes(' ')){
            parsedData = parsedData.replaceAll(' ', '%20');
        }
        if(parsedData.includes('!')){
            parsedData = parsedData.replaceAll('!', '%21');
        }
        if(parsedData.includes('#')){
            parsedData = parsedData.replaceAll('#', '%23');
        }
        if(parsedData.includes('$')){
            parsedData = parsedData.replaceAll('$', '%24');
        }
        if(parsedData.includes('%')){
            parsedData = parsedData.replaceAll('%', '%25');
        }
        if(parsedData.includes('&')){
            parsedData = parsedData.replaceAll('&', '%26');
        }
        if(parsedData.includes('+')){
            parsedData = parsedData.replaceAll('+', '%2B');
        }
        return parsedData;
    }

    const handleClick = (e) => {
        var value = e.target.getAttribute('data-item');
        var parsedData = handleEncoded(value)
        axios.post("https://localhost:7010/Tag/deleteTag?tagName=" + parsedData)
        .then((response => {
            fetchTableData();
            
        }))
        .catch((err => {
            console.log(err);
        }))
    }

    const handleChange = (e) => {
        setAlertData({message: ''});
        setCreateData(e.target.value);
    }
     
    const fetchTableData = () => {
        async function fetchData() {
            const request = await axios.get("https://localhost:7010/Tag/taglist")
            .then((response => {
                setData(response);
            }))
            .catch((err => {
                switch(err.response.status){
                    case 401: {
                            console.log("Not Authorized");
                            localStorage.removeItem('authorization');
                            window.location.assign(window.location.origin);
                            window.location = '/';
                    }
                        break;
                    case 503: {
                            console.log("Database offline");
                            
                    }
                        break;
                }
            }))
            
            
        }
        fetchData();
    }

    const checkToken = () => {
        const token = localStorage.getItem('authorization');
        
    }

    useEffect(() => {
        checkToken();
        fetchTableData();
        //Refresh after every 5 seconds
        const interval = setInterval(() => {
            checkToken();
            fetchTableData();   
        }, 5000);
        return () => clearInterval(interval);
    }, [])

    const createTag = (e) => {
        e.preventDefault();
        var parsedData = handleEncoded(createData);
        
       

        console.log(parsedData);

        axios.post("https://localhost:7010/Tag/createTag?tagName=" + parsedData)
        .then((response => {
            setCreateData('');
            setAlertData({message: 'Added'});
            fetchTableData();
            setTimeout(() => {
                setAlertData({message: ''});
            }, 1000)
            
        }))
        .catch((err => {
            switch(err.response.status){
                case 409: setAlertData({message: 'Tag already exists'});
                    break;
                case 401: {
                        console.log("Not Authorized");
                        localStorage.removeItem('authorization');
                        window.location.assign(window.location.origin);
                        window.location = '/';
                }
                    break;
                case 503: {
                        console.log("Database offline");
                        setAlertData({message: 'Database offline'});
                }
                    break;
                default: 
                    setAlertData({message: 'Unable to create tag'});
            }
        }))
    }

    const renderTable = (
        <div className="tag-dashboard-table-container">
            <table className = "tag-dashboard-table">
                <thead className = "tag-dashboard-table-thead">
                    <tr>
                        <th>Tags</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map(item =>{
                        return(
                            <tr key={item.tagName} className = "row-tag-table">                          
                                        <td className = "table-item-name" data-item = {item.tagName} ><span className = "delete-icon-tag-table" data-item = {item.tagName} onClick = {handleClick}> &#10006;&emsp;</span>{item.tagName}</td>
                                        <td data-item = {item.tagName} >{item.tagCount}</td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );

    const renderForm = (
        <div className = "tag-dashboard-form-container">
            <form className="tag-dashboard-create-form" onSubmit = {createTag}>
                    <div className="tag-dashboard-create-input-container">
                        <input type="text" value={createData} required placeholder="Tag Name" onChange = {handleChange}/>
                    </div>
                    <div className = "tag-dashboard-create-button">
                        <Button type="button" color="green" name="Create"/>
                    </div> 
                    <div className="create-tag-error"> {alertData.message} </div>
                </form>
        </div>
    );

    const renderBack = (
        <div className = "tag-dashboard-back-container">
            <a href="/Admin/Dashboard">&lt;&emsp;Return to Portal</a>
        </div>
    );

    return (
        
        <div className="tag-dashboard-wrapper">
            <div className = "tag-dashboard-nav-wrapper">
                {<AuthenticatedNavBar/>}
            </div>
            <div className = "tag-dashboard-back-wrapper">
                {renderBack}
            </div>
            <div className = "tag-dashboard-table-wrapper">
                {renderTable}
            </div>
            <div className = "tag-dashboard-form-wrapper">
                {renderForm}
            </div>
        </div>
    )
}

export default TagDashboard;