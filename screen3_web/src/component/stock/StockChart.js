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
    ema10: false,
    ema20: false,
    ema50: true,
    sma100: false,
    sma200: true,
    bb: true,
    macd: true,
    adx: true,
    heikin: false,
    stochastic: false,
    rsi: false,
    william: true,
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

  onIndicatorChange = (ind) => {
    const setting = Object.assign(this.currentChartSettings);

    setting[ind] = !setting[ind];
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
              </div>
              <div className="row">
                <span>
                  <label className="radio-inline">
                    <input
                      type="radio"
                      name="type"
                      checked={state.type === "day"}
                      onChange={() => this.onTypeChange("day")}
                    />
                    Day
                  </label>
                  <label className="radio-inline">
                    <input
                      type="radio"
                      name="type"
                      checked={state.type === "week"}
                      onChange={() => this.onTypeChange("week")}
                    />
                    Week
                  </label>
                </span>
                <span style={{ paddingLeft: 20 }}>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="ema10"
                      checked={state.ema10}
                      onChange={() => {
                        this.onIndicatorChange("ema10");
                      }}
                    />
                    ema10
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="ema20"
                      checked={state.ema20}
                      onChange={() => {
                        this.onIndicatorChange("ema20");
                      }}
                    />
                    ema20
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="ema50"
                      checked={state.ema50}
                      onChange={() => {
                        this.onIndicatorChange("ema50");
                      }}
                    />
                    ema50
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="sma100"
                      checked={state.sma100}
                      onChange={() => {
                        this.onIndicatorChange("sma100");
                      }}
                    />
                    sma100
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="sma200"
                      checked={state.sma200}
                      onChange={() => {
                        this.onIndicatorChange("sma200");
                      }}
                    />
                    sma200
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="bb"
                      checked={state.bb}
                      onChange={() => {
                        this.onIndicatorChange("bb");
                      }}
                    />
                    bb
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="macd"
                      checked={state.macd}
                      onChange={() => {
                        this.onIndicatorChange("macd");
                      }}
                    />
                    macd
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="adx"
                      checked={state.adx}
                      onChange={() => {
                        this.onIndicatorChange("adx");
                      }}
                    />
                    adx
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="heikin"
                      checked={state.heikin}
                      onChange={() => {
                        this.onIndicatorChange("heikin");
                      }}
                    />
                    heikin
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="stochastic"
                      checked={state.stochastic}
                      onChange={() => {
                        this.onIndicatorChange("stochastic");
                      }}
                    />
                    stochastic
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="rsi"
                      checked={state.rsi}
                      onChange={() => {
                        this.onIndicatorChange("rsi");
                      }}
                    />
                    rsi
                  </label>
                  <label class="checkbox-inline">
                    <input
                      type="checkbox"
                      value="william"
                      checked={state.william}
                      onChange={() => {
                        this.onIndicatorChange("william");
                      }}
                    />
                    wr
                  </label>
                </span>
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
