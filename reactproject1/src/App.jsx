import React from 'react';
import Header from './Components/Header';
import Login from './Components/Login';
import Services from './Components/Services';

export default class App extends React.Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = { pageSize: 5, pageNumber: 1 };
    }
    
    render() {
        return (
            <div class="w3-container">
                <Login />
                <Services pageSize={this.state.pageSize} pageNumber={this.state.pageNumber} />
            </div>
        );
    }

}
