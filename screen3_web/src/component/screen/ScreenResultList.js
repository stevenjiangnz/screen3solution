import React, { Component } from "react";
import AppContext from "../../Context";

export class ScreenResultList extends Component {
  context;

  componentDidUpdate() {
    console.log("in component did update...");
  }

  render() {
    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;
          return <div>{context.state.screenResult.length}</div>;
        }}
      </AppContext.Consumer>
    );
  }
}

export default ScreenResultList;
