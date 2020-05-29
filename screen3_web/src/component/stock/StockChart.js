import React, { Component } from "react";
import AppContext from "../../Context";
import Highcharts from "highcharts/highstock";
import HighchartsReact from "highcharts-react-official";
import TickerService from "../../service/TickerService";
import IndicatorService from "../../service/IndicatorService";
import ChartSettings from "../../ChartSettings";
import TickerHelper from "../../util/TickerHelper";

export class StockChart extends Component {
  context;
  chartRef;
  chart;
  tickerService;
  indicatorService;
  groupingUnits;

  constructor(props) {
    super(props);

    this.tickerService = new TickerService();
    this.indicatorService = new IndicatorService();
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

    // const tickersReturn = await this.tickerService.getTickerList(stock.code);
    // const tickers = TickerHelper.ConvertTickers(tickersReturn.data);
    // const sma20Return = await this.indicatorService.getSMA(stock.code, 50);
    // const sma20 = TickerHelper.ConvertSingleValueIndicator(sma20Return.data);

    const getTickers = this.tickerService.getTickerList(stock.code);
    const getSMA20 = this.indicatorService.getSMA(stock.code, 50);

    Promise.all([getTickers, getSMA20]).then((values) => {
      const tickers = TickerHelper.ConvertTickers(values[0].data);
      const sma20 = TickerHelper.ConvertSingleValueIndicator(values[1].data);
      // display candlestick
      this.chart.setTitle({ text: `${stock.code} - ${stock.company}` });
      this.chart.series[0].setData(tickers);
      this.chart.series[0].name = stock.code + "- price";

      this.drawIndicator("SMA20", sma20, { yAxis: 0 });
    });
  };

  drawIndicator = (name, data, settings = {}) => {
    var existingSeries = null;

    this.chart.series.forEach((series) => {
      if (series.name === name) {
        existingSeries = series;
      }
    });

    if (!existingSeries) {
      this.chart.addSeries({
        name,
        type: settings.type ? settings.type : "line",
        data,
        yAxis: settings.yAxis ? settings.yAxis : 0,
      });
    } else {
      console.log("about to set data to existing serious: ", existingSeries);
      existingSeries.setData(data);
    }
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
