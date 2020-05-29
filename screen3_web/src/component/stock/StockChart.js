import React, { Component } from "react";
import AppContext from "../../Context";
import Highcharts, { objectEach } from "highcharts/highstock";
import HighchartsReact from "highcharts-react-official";
import TickerService from "../../service/TickerService";
import IndicatorService from "../../service/IndicatorService";
import ChartConfig from "../../ChartConfig";
import TickerHelper from "../../util/TickerHelper";

export class StockChart extends Component {
  context;
  chartRef;
  chart;
  tickerService;
  indicatorService;
  groupingUnits;
  chartName;
  currentChartSettings;

  defaultChartSetting = {
    type: "day",
  };

  constructor(props) {
    super(props);
    this.chartName = props.name;
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

    const getTickers = this.tickerService.getTickerList(
      stock.code,
      this.currentChartSettings.type
    );
    const getSMA20 = this.indicatorService.getSMA(
      stock.code,
      20,
      this.currentChartSettings.type
    );

    Promise.all([getTickers, getSMA20]).then((values) => {
      const tickers = TickerHelper.ConvertTickers(values[0].data);
      const sma20 = TickerHelper.ConvertSingleValueIndicator(values[1].data);

      this.chart.setTitle({ text: `${stock.code} - ${stock.company}` });
      this.chart.series[0].setData(tickers);
      this.chart.series[0].name = `${stock.code} - ${this.currentChartSettings.type}`;

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
        lineWidth: 1,
        color: "red",
        yAxis: settings.yAxis ? settings.yAxis : 0,
      });
    } else {
      existingSeries.setData(data);
    }
  };

  testClicked = () => {
    this.chart.xAxis[0].setExtremes(
      Date.UTC(2014, 0, 1),
      Date.UTC(2014, 11, 31)
    );
  };

  onTypeChange = (type) => {
    const setting = Object.assign(this.currentChartSettings);

    setting["type"] = type;
    this.context.updateChartSettings(this.chartName, setting);
  };

  render() {
    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;
          this.currentChartSettings = context.state[this.chartName]
            ? context.state[this.chartName]
            : this.defaultChartSetting;

          const state = this.currentChartSettings;
          return (
            <>
              <div className="row">
                <p>{JSON.stringify(this.context)}</p>
                <button className="btn btn-primary" onClick={this.testClicked}>
                  {this.chartName}
                </button>

                <label className="radio-inline">
                  <input
                    type="radio"
                    name="optradio"
                    checked={state.type === "day"}
                    onChange={() => this.onTypeChange("day")}
                  />
                  Day
                </label>
                <label className="radio-inline">
                  <input
                    type="radio"
                    name="optradio"
                    checked={state.type === "week"}
                    onChange={() => this.onTypeChange("week")}
                  />
                  Week
                </label>
              </div>
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
