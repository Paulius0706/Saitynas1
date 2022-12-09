import React from 'react';

export default class CostumeCheckBox extends React.Component {
    static displayName = CostumeCheckBox.name;

    constructor(props) {
        super(props)
        //this.handleChange.bind(this);
    }

    //handleChange = (e) => { this.props.onChange(e.target.value, e.target.value) = ; }

    render() {
        return (
            <>
                <label>{this.props.name}</label>
                <input type="checkbox" id={this.props.id} value={this.props.value} onclick={this.props.onChange} /><br />
            </>

        );
    }
}