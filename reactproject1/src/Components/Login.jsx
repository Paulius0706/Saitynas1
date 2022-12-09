import React from 'react';
import CmdButton from './CmdButton';
import CostumeTextBox from './CostumeTextBox';

export default class Login extends React.Component {
    static displayName = Login.name;

    constructor(props) {
        super(props);
        this.state = { item: { name: 'names111', password: 'Password1-', }, token: '', loggedIn: false };
        this.modify.bind(this);
    }

    modify = (e) => { let item = this.state.item; item[e.target.id] = e.target.value; this.setState({ item: item }); }
    login = () => { this.TryLogin(this.state.item.name, this.state.item.password); }
    logout = () => { localStorage.setItem('token', ''); this.setState({ loggedIn: false, item: { name: '', password: '' }, token: '' }); }

    render() {
        let loginInfo = this.state.loggedIn
            ? <><h4>Logged as: {this.state.item.name}</h4></>
            : <><h4>No login</h4></>
        let loginHtml = this.state.loggedIn
            ? <>
                <CmdButton onClick={this.logout} text={'Logout'} />
            </>

            : <><CostumeTextBox id={"name"} name={"LoginName"} value={this.state.item.name} onChange={(e) => { this.modify(e); }} />
                <CostumeTextBox id={"password"} name={"Password"} value={this.state.item.password} onChange={(e) => { this.modify(e); }} />
                <CmdButton onClick={this.login} text={'Login'} />
            </>
        return (
            <div>
                <h3>Login</h3>
                {loginHtml}
                {loginInfo}
            </div>
        );
    }
    async TryLogin(name, password) {
        let res = await fetch('api/login', {
            method: "POST",
            headers: new Headers({
                'Content-Type': 'application/json'
            }),
            body: JSON.stringify({
                UserName: name,
                Password: password
            })
        });
        if (res.status === 200) {
            
            const data = await res.json();
            localStorage.setItem('token', 'Bearer ' + data.accessToken);
            this.setState({ token: 'Bearer ' + data.accessToken, loggedIn: true });
        } else {
            localStorage.setItem('token', '');
            this.setState({loggedIn: false });
        }
        
    }

}