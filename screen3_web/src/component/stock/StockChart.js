import React, { Component } from "react";
import AppContext from "../../Context";
import Highcharts from "highcharts/highstock";
import HighchartsReact from "highcharts-react-official";
import TickerServer from "../../service/TickerService";

export class StockChart extends Component {
  context;
  chartRef;
  chart;
  tickerService;
  groupingUnits;

  constructor(props) {
    super(props);

    this.tickerService = new TickerServer();
    this.chartRef = React.createRef();

    this.groupingUnits = [
      [
        "week", // unit name
        [1], // allowed multiples
      ],
      ["month", [1, 2, 3, 4, 6]],
    ];
    this.state = {
      chart: {
        marginRight: 20,
        marginLeft: 10,
      },
      series: [
        {
          type: "candlestick",
          data: [],
          groupingUnits: this.groupingUnits,
        },
      ],
      rangeSelector: {
        selected: 1,
      },

      screen_setting: "",
    };
  }

  componentDidMount() {
    this.chart = this.chartRef.current.chart;
    this.prepareDrawChart();
  }

  componentDidUpdate(prevProps, prevState) {
    this.prepareDrawChart();
  }

  prepareDrawChart = async () => {
    const stock = this.context.state.selectedStock;

    const tickers = await this.tickerService.getTickerList(stock.code);

    this.chart.setTitle({ text: `${stock.code} - ${stock.company}` });

    this.chart.series[0].setData(tickers);
    this.chart.series[0].name = stock.code;
  };

  render() {
    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;
          return (
            <>
              <div className="chart-wrapper">
                <div className="chart-inner">
                  <HighchartsReact
                    highcharts={Highcharts}
                    constructorType={"stockChart"}
                    options={this.state}
                    ref={this.chartRef}
                  />
                </div>
              </div>
            </>
          );
        }}
      </AppContext.Consumer>
    );
  }
}

export default StockChart;
