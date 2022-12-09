import React from 'react';

export default class PlaceHolder extends React.Component {
    static displayName = PlaceHolder.name;

    // id, name, value, type, show
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <>
                <label> {this.props.name}</label> {this.props.value} <br />
            </>
        );
    }
}