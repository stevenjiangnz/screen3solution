import React, { Component } from "react";
import StockService from "../../service/StockService";
import { AgGridReact } from "ag-grid-react";
import AppContext from "../../Context";

export class StockList extends Component {
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
        { headerName: "Code", field: "code", width: 60 },
        { headerName: "Company", field: "company" },
        { headerName: "Sector", field: "sector", width: 120 },
        { headerName: "Weight", field: "weight", width: 70 },
      ],
    };
  }

  onFilterChanged = (e) => {
    const grid = this.adRef.current;
    grid.api.setQuickFilter(e.target.value);
  };

  onRowClicked = (e) => {
    this.context.updateSelectedStock(e.data);
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
                  rowData={this.context.state.stockList}
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

export default StockList;
