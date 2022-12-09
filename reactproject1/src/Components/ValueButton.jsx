import React from 'react';

export default class ValueButton extends React.Component {
    static displayName = ValueButton.name;

    constructor(props) {
        super(props)
        this.handleClick.bind(this);
    }

    handleClick = () => { this.props.onClick(this.props.value); }

    render() { return ( <button onClick={this.handleClick}>{this.props.text}</button> ); }
}