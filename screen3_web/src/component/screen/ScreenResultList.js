import React, { Component } from "react";
import { AgGridReact } from "ag-grid-react";
import AppContext from "../../Context";

export class ScreenResultList extends Component {
  context;
  adRef;
  constructor(props) {
    super(props);
    this.adRef = React.createRef();

    this.state = {
      defaultColDef: {
        sortable: true,
        resizable: true,
        cellClass: "ag-cell",
      },
      columnDefs: [
        {
          headerName: "Code",
          field: "code",
          width: 70,
        },
        { headerName: "Direction", field: "direction", width: 90 },
        { headerName: "Date", field: "p", width: 120 },
        { headerName: "Stamp", field: "p_Stamp", hide: true },
      ],
    };
  }

  onFilterChanged = (e) => {
    const grid = this.adRef.current;
    grid.api.setQuickFilter(e.target.value);
  };

  onRowClicked = (e) => {
    this.context.updateCurrentScreenStock(e.data);
    // this.props.onItemClicked();
  };

  render() {
    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;
          return (
            <div
              className="ag-theme-balham ag-grid-container"
              style={{ marginTop: "2px" }}
            >
              <input
                type="text"
                className="form-control"
                placeholder="filter"
                onChange={this.onFilterChanged}
              ></input>
              <div id="grid-container">
                <AgGridReact
                  defaultColDef={this.state.defaultColDef}
                  columnDefs={this.state.columnDefs}
                  rowData={this.context.state.screenResult}
                  quickFilter={this.state.filterText}
                  ref={this.adRef}
                  onRowClicked={this.onRowClicked}
                ></AgGridReact>
              </div>
            </div>
          );
        }}
      </AppContext.Consumer>
    );
  }
}

export default ScreenResultList;
