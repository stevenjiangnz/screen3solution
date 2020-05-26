import React, { Component } from "react";
import RequestHelper from "../../util/RequestHelper";
import { AgGridReact } from "ag-grid-react";
import "ag-grid-community/dist/styles/ag-grid.css";
import "ag-grid-community/dist/styles/ag-theme-balham.css";

export class StockList extends Component {
  constructor(props) {
    super(props);

    this.adRef = React.createRef();

    this.state = {
      filterText: "",
      defaultColDef: {
        sortable: true,
        filter: true,
        resizable: true,
      },
      columnDefs: [
        { headerName: "Code", field: "code", width: 60 },
        { headerName: "Company", field: "company", width: 150 },
        { headerName: "Sector", field: "sector", width: 120 },
        { headerName: "Weight", field: "weight" },
      ],
      stocks: [],
    };
  }

  onFilterChanged = (e) => {
    const grid = this.adRef.current;
    grid.api.setQuickFilter(e.target.value);
    console.log("button clicked", e.target.value);
  };

  componentDidMount() {
    const req = new RequestHelper().getIntance();

    req
      .get("stock")
      .then((resp) => {
        this.setState({
          stocks: resp.data,
        });
      })
      .catch((error) => {
        console.log("error", error);
      });
  }
  render() {
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

        <AgGridReact
          defaultColDef={this.state.defaultColDef}
          columnDefs={this.state.columnDefs}
          rowData={this.state.stocks}
          domLayout="autoHeight"
          quickFilter={this.state.filterText}
          ref={this.adRef}
        ></AgGridReact>
      </div>
    );
  }
}

export default StockList;
