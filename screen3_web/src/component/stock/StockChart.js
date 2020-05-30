import React, { Component } from "react";
import AppContext from "../../Context";
import Highcharts from "highcharts/highstock";
import HighchartsReact from "highcharts-react-official";
import TickerService from "../../service/TickerService";
import IndicatorService from "../../service/IndicatorService";
import TickerHelper from "../../util/TickerHelper";
import ChartHelper from "../../util/ChartHelper";

export class StockChart extends Component {
  context;
  chartRef;
  chart;
  tickerService;
  indicatorService;
  groupingUnits;
  chartName;
  currentChartSettings;

  defaultChartSetting = ChartHelper.getChartDefaultSettins();

  constructor(props) {
    super(props);
    this.chartName = props.name;
    this.tickerService = new TickerService();
    this.indicatorService = new IndicatorService();
    this.chartRef = React.createRef();

    this.groupingUnits = ChartHelper.getGroupUnit();

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
    const indicators = ChartHelper.getOnIndicators(this.currentChartSettings);
    const dataTasks = [];

    dataTasks.push(
      this.tickerService
        .getTickerList(stock.code, this.currentChartSettings.type)
        .then((value) => TickerHelper.ConvertTickers(value.data))
    );

    indicators.forEach((ind) => {
      const param = ind.parameter.split(",");
      switch (param[0]) {
        case "sma":
          dataTasks.push(
            this.indicatorService
              .getSMA(stock.code, param[1], this.currentChartSettings.type)
              .then((value) =>
                TickerHelper.ConvertSingleValueIndicator(value.data)
              )
          );
          break;
        case "ema":
          dataTasks.push(
            this.indicatorService
              .getEMA(stock.code, param[1], this.currentChartSettings.type)
              .then((value) =>
                TickerHelper.ConvertSingleValueIndicator(value.data)
              )
          );
          break;
        case "bb":
          break;
        case "macd":
          break;
        case "adx":
          break;
        case "heikin":
          break;
        case "stochastic":
          break;
        case "rsi":
          break;
        case "william":
          break;
        default:
          console.error(`found unknown indicator parameter ${param[0]}`);
      }
    });

    Promise.all([...dataTasks]).then((values) => {
      const tickers = values[0];

      this.chart.setTitle({ text: `${stock.code} - ${stock.company}` });
      this.chart.series[0].setData(tickers);
      this.chart.series[0].name = `${stock.code} - ${this.currentChartSettings.type}`;

      for (var i = 1; i < values.length; i++) {
        const indSetting = indicators[i - 1];
        this.drawIndicator(indSetting.name, values[i], indSetting);
      }

      // Clear
      const allInds = Object.keys(this.currentChartSettings);
      const onIndicators = indicators.map((ind) => ind.name);
      this.chart.series.forEach((s) => {
        if (allInds.includes(s.name)) {
          if (!onIndicators.includes(s.name)) {
            s.remove();
          }
        }
      });
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
      this.chart.addSeries(
        {
          name,
          type: settings.chartType ? settings.chartType : "line",
          data,
          lineWidth: 1,
          color: settings.color,
          yAxis: settings.yAxis ? settings.yAxis : 0,
        },
        true
      );
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
              {/* <div className="row">
                <p>{JSON.stringify(this.context)}</p>
                <button className="btn btn-primary" onClick={this.testClicked}>
                  {this.chartName}
                </button>
              </div> */}
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
                  <label className="checkbox-inline">
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
                  <label className="checkbox-inline">
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
                  <label className="checkbox-inline">
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
                  <label className="checkbox-inline">
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
                  <label className="checkbox-inline">
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
                  <label className="checkbox-inline">
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
                  <label className="checkbox-inline">
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
                  <label className="checkbox-inline">
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
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="stochastic"
                      checked={state.stochastic}
                      onChange={() => {
                        this.onIndicatorChange("stochastic");
                      }}
                    />
                    stoch
                  </label>
                  <label className="checkbox-inline">
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
                  <label className="checkbox-inline">
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
