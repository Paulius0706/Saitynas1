import React from 'react';

export default class CostumeDateTime extends React.Component {
    static displayName = CostumeDateTime.name;

    constructor(props) {
        super(props)
        //this.handleChange.bind(this);
    }

    //handleChange = (e) => { this.props.onChange(e.target.value, e.target.value) = ; }

    render() {
        return (
            <>
                <label>{this.props.name}</label>
                <input type={"datetime-local"} id={this.props.id} value={this.props.value} onChange={this.props.onChange} /> <br />
            </>

        );
    }
}