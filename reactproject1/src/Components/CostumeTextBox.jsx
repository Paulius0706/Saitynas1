import React from 'react';

export default class CostumeTextBox extends React.Component {
    static displayName = CostumeTextBox.name;

    constructor(props) {
        super(props)
        //this.handleChange.bind(this);
    }

    //handleChange = (e) => { this.props.onChange(e.target.value, e.target.value) = ; }

    render() {
        return (
            <>
                <label>{this.props.name}</label>
                <input type={"text"} id={this.props.id} value={this.props.value} onChange={this.props.onChange} /> <br/>
            </>
            
        );
    }
}