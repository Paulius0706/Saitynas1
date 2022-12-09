import React from 'react';



export default class CmdButton extends React.Component {
    static displayName = CmdButton.name;

    constructor(props) {
        super(props)
        this.handleClick.bind(this);
    }

    handleClick = () => {
        this.props.onClick();
    }

    render() {
        return (
            <button onClick={this.handleClick}>{this.props.text}</button>
        );
    }
}