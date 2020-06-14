import React, { Component } from "react";
import { AgGridReact } from "ag-grid-react";

export class TradeClosedPositions extends Component {
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
          width: 55,
        },
        { headerName: "D", field: "direction", width: 15 },
        { headerName: "E-Date", field: "entryDate", width: 90 },
        { headerName: "E-Price", field: "entryPrice", width: 70 },
        { headerName: "P/L", field: "pl", width: 70 },
        { headerName: "X-Date", field: "exitDate", width: 90 },
        { headerName: "X-Price", field: "exitPrice", width: 70 },
      ],
    };
  }

  getSummary = (trades) => {
    var summary = {
      long: 0,
      short: 0,
      win: 0,
      loss: 0,
      draw: 0,
      pl: 0,
    };
    trades.forEach((tr) => {
      summary.pl += parseFloat(tr.pl);
      if (tr.direction > 0) {
        summary.long++;
      } else {
        summary.short++;
      }

      if (tr.pl > 0) {
        summary.win++;
      }

      if (tr.pl < 0) {
        summary.loss++;
      }

      if (tr.pl === 0) {
        summary.draw++;
      }
    });

    return summary;
  };

  render() {
    const { trades } = this.props;
    const summary = this.getSummary(trades);

    return (
      <div style={{ marginTop: 10 }}>
        <h5>Close Positions ({trades?.length}):</h5>
        <span>{`PL: ${summary.pl}`}</span>
        <span
          style={{ marginLeft: 30 }}
        >{`W/L/D : ${summary.win} / ${summary.loss} / ${summary.draw}`}</span>
        <span
          style={{ marginLeft: 30 }}
        >{`L/S: ${summary.loss} / ${summary.short}`}</span>

        <div
          className="ag-theme-balham ag-grid-container"
          style={{ marginTop: "2px" }}
        >
          <div id="grid-container-position">
            <AgGridReact
              defaultColDef={this.state.defaultColDef}
              columnDefs={this.state.columnDefs}
              rowData={trades}
              ref={this.adRef}
            ></AgGridReact>
          </div>
        </div>
      </div>
    );
  }
}

export default TradeClosedPositions;
