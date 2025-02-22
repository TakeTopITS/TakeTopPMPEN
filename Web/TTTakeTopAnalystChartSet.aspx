<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTTakeTopAnalystChartSet.aspx.cs" Inherits="TTTakeTopAnalystChartSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .card-container {
            display: flex;
            gap: 10px;
            vertical-align: bottom;
        }

        .card {
            width: 100%;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            text-align: center;
        }

            .card.blue {
                background-color: #367E7F;
            }

            .card.red {
                background-color: #d9534f;
            }

            .card.green {
                background-color: #5cb85c;
            }

            .card.lightblue {
                background-color: #7092BE;
            }

            .card.brown {
                background-color: #818049;
            }

            .card img {
                max-width: 30px;
                margin-bottom: 10px;
            }

            .card h3 {
                margin: 0;
                font-size: 18px;
                color: white;
            }

            .card p {
                margin: 5px 0;
                font-size: 14px;
                color: white;
            }
    </style>
    <script src="js/jquery-1.10.2.min.js"></script>
</head>

<body>
    <form id="form1" runat="server">
        <div style="width: 100%; text-align: center;">
            <div id="m2" style="width: 100%; height: 270px; overflow: hidden;"></div>
        </div>
    </form>


    <script src="Scripts/ECharts/echarts-all.js"></script>

    <script type="text/javascript">

        //取得URL参数的值
        function GetQueryValue(queryName) {
            var query = decodeURI(window.location.search.substring(1));
            var vars = query.split('&');
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split('=');
                if (pair[0] == queryName) { return pair[1]; }
            }
            return null;
        }

        $(function () {

            var myChart1 = echarts.init(document.getElementById('m2'));

            var formType = GetQueryValue("FormType");
            var chartType = GetQueryValue("ChartType");
            var chartName = GetQueryValue("ChartName");
            var sqlCode = escape(unescape(GetQueryValue("SqlCode")));

            //仪表盘
            if (chartType == 'Gauge') {
                var option1 = {
                    title: {
                        text: chartName,
                        subtext: '',
                        itemGap: 8,
                        x: 'center',
                        y: 'center',
                        textStyle: {
                            color: '#000000',
                            fontSize: 10,
                        },
                        subtextStyle: {
                            color: '#000000',
                            fontSize: 8
                        },
                    },
                    tooltip: {
                        trigger: 'item',
                        transitionDuration: 8,
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    noDataLoadingOption: {
                        text: '暂无数据（No Data）',
                        effect: 'bubble',
                        effectOption: {
                            effect: {
                                n: 0
                            }
                        }
                    },
                    series: [
                        {
                            name: '',
                            type: 'gauge',
                            /* axisTick: false,//是否显示刻度*/
                            //pointer: {
                            //    show: false//是否显示指针
                            //},
                            splitLine: {
                                show: false,//是否显示分隔线。
                            },
                            axisLabel: false,
                            radius: '55%',
                            center: ['50%', '30%'],
                            //detail: {
                            //    formatter: '{value}'
                            //},
                            data: []
                        }
                    ]
                };
                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {

                        if (result) {
                            eval("var transresult=" + result);

                            var eLegend = new Array();
                            var eSeries = new Array();

                            for (var i = 0; i < transresult.length; i++) {

                                /*   eLegend.push(transresult[i].XName);*/
                                eSeries.push({
                                    value: transresult[i].YNumber,
                                    name: transresult[i].XName
                                });

                            }

                            /*   option1.legend.data = eLegend;*/
                            option1.series[0].data = eSeries;

                            myChart1.setOption(option1);
                        }
                    },
                    error: function (errorMsg) {
                        //alert("request data failed!!!");
                    }
                });
            }


            //饼图
            if (chartType == 'Pie') {
                var option1 = {
                    title: {
                        text: chartName,
                        subtext: '',
                        itemGap: 8,
                        x: 'center',
                        y: 'center',
                        textStyle: {
                            color: '#000000',
                            fontSize: 10,
                        },
                        subtextStyle: {
                            color: '#000000',
                            fontSize: 8
                        },
                    },
                    tooltip: {
                        trigger: 'item',
                        transitionDuration: 8,
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    legend: {
                        orient: 'vertical',
                        x: 'left',
                        data: []
                    },
                    toolbox: {
                        show: false,
                        feature: {
                            mark: { show: true },
                            dataView: { show: true, readOnly: false },
                            magicType: {
                                show: true,
                                type: ['pie', 'funnel'],
                                option: {
                                    funnel: {
                                        x: '25%',
                                        width: '50%',
                                        funnelAlign: 'left',
                                        max: 1548
                                    }
                                }
                            },
                            restore: { show: true },
                            saveAsImage: { show: true }
                        }
                    },
                    noDataLoadingOption: {
                        text: '暂无数据（No Data）',
                        effect: 'bubble',
                        effectOption: {
                            effect: {
                                n: 0
                            }
                        }
                    },


                    series: [
                        {
                            name: '',
                            type: 'pie',
                            radius: '50%',
                            center: ['50%', '30%'],
                            data: [],
                            itemStyle: {
                                normal: {
                                    // color: 各异,
                                    borderWidth: 1,
                                    label: {
                                        show: true,//数据标签显示
                                        position: 'outer',
                                        textStyle://数据标签的字体配置，与其他组件相同
                                        {
                                            fontSize: 10,//字号
                                            fontWeight: 'normal',//粗细【normal\bold\bolder\lighter】
                                            fontFamily: 'Microsoft YaHei',//字体【 'serif'\'monospace'\'Arial'\'Courier New'\'Microsoft YaHei'】
                                            color: ''//颜色各异
                                        },
                                        formatter: '{b}'//a:系列名，就是你上面写的name半径模型，b：数据名，就是rose1，c：数据值，d百分比
                                    },
                                    labelLine: {
                                        show: true,//数据标签引导线
                                        length: 5,
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid'
                                        }
                                    }
                                },
                                //emphasis: {//选中的样式
                                //    borderColor: 'rgba(0,0,0,0)',
                                //    borderWidth: 1,
                                //    label: {
                                //        show: true//选中时不显示数据标签
                                //    },
                                //    labelLine: {
                                //        show: true,//选中时不显示数据标签引导线
                                //        length: 5,
                                //        lineStyle: {
                                //            width: 1,
                                //            type: 'solid'
                                //        }
                                //    }
                                //}
                            }


                        },
                    ]
                };
                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {

                        if (result) {
                            eval("var transresult=" + result);

                            var eLegend = new Array();
                            var eSeries = new Array();

                            for (var i = 0; i < transresult.length; i++) {

                                eLegend.push(transresult[i].XName);
                                eSeries.push({
                                    value: transresult[i].YNumber,
                                    name: transresult[i].XName
                                });

                            }

                            /*     option1.legend.data = eLegend;*/
                            option1.series[0].data = eSeries;

                            myChart1.setOption(option1);
                        }
                    },
                    error: function (errorMsg) {
                        //alert("request data failed!!!");
                    }
                });
            }

            //圈图
            if (chartType == 'Doughnut') {

                var option1 = {
                    title: {
                        text: chartName,
                        subtext: '',
                        itemGap: 8,
                        x: 'center',
                        y: 'center',
                        textStyle: {
                            color: '#000000',
                            fontSize: 10,
                        },
                        subtextStyle: {
                            color: '#000000',
                            fontSize: 8
                        },

                    },
                    tooltip: {
                        trigger: 'item',
                        transitionDuration: 8,
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    //legend: {
                    //    top: '5%',
                    //    left: 'center'
                    //},

                    noDataLoadingOption: {
                        text: '暂无数据（No Data）',
                        effect: 'bubble',
                        effectOption: {
                            effect: {
                                n: 0
                            }
                        }
                    },
                    series: [
                        {
                            name: '',
                            type: 'pie',
                            radius: ['30%', '50%'],
                            center: ['50%', '30%'],
                            avoidLabelOverlap: false,
                            itemStyle: {
                                borderRadius: 10,
                                borderColor: '#fff',
                                borderWidth: 2
                            },
                            label: {
                                show: false,
                                position: 'center'
                            },
                            emphasis: {
                                label: {
                                    show: true,
                                    fontSize: '8',
                                    fontWeight: 'bold'
                                }
                            },
                            labelLine: {
                                show: false
                            },
                            data: [],
                            itemStyle: {
                                normal: {
                                    // color: 各异,
                                    borderWidth: 1,
                                    label: {
                                        show: true,//数据标签显示
                                        position: 'outer',
                                        textStyle://数据标签的字体配置，与其他组件相同
                                        {
                                            fontSize: 10,//字号
                                            fontWeight: 'normal',//粗细【normal\bold\bolder\lighter】
                                            fontFamily: 'Microsoft YaHei',//字体【 'serif'\'monospace'\'Arial'\'Courier New'\'Microsoft YaHei'】
                                            color: ''//颜色各异
                                        },
                                        formatter: '{b}'//a:系列名，就是你上面写的name半径模型，b：数据名，就是rose1，c：数据值，d百分比
                                    },
                                    labelLine: {
                                        show: true,//数据标签引导线
                                        length: 5,
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid'
                                        }
                                    }
                                },
                                //emphasis: {//选中的样式
                                //    borderColor: 'rgba(0,0,0,0)',
                                //    borderWidth: 1,
                                //    label: {
                                //        show: true//选中时不显示数据标签
                                //    },
                                //    labelLine: {
                                //        show: true,//选中时不显示数据标签引导线
                                //        length: 5,
                                //        lineStyle: {
                                //            width: 1,
                                //            type: 'solid'
                                //        }
                                //    }
                                //}
                            }
                        }
                    ]
                };
                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);

                            var eLegend = new Array();
                            var eSeries = new Array();

                            for (var i = 0; i < transresult.length; i++) {

                                eLegend.push(transresult[i].XName);
                                eSeries.push({
                                    value: transresult[i].YNumber,
                                    name: transresult[i].XName
                                });

                            }

                            /*     option1.legend.data = eLegend;*/
                            option1.series[0].data = eSeries;

                            myChart1.setOption(option1);
                        }
                    },
                    error: function (errorMsg) {
                        //alert("request data failed!!!");
                    }
                });
            }

            //纵向柱状图
            if (chartType == 'Column') {
                var option1 = {
                    title: {
                        text: chartName,
                        subtext: '',
                        x: 'center',
                        y: 'center',
                        textStyle: {
                            color: '#000000',
                            fontSize: 10,
                        },
                        subtextStyle: {
                            color: '#000000',
                            fontSize: 8
                        },
                    },
                    tooltip: {
                        trigger: 'axis',
                        transitionDuration: 8,
                        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    //toolbox: {
                    //    show: false,
                    //    feature: {
                    //        saveAsImage: {
                    //            show: true
                    //        }
                    //    }
                    //},
                    //legend: {
                    //    data: ['data'],
                    //    right: '5%'
                    //},
                    grid: {
                        x: 40,
                        y: 50,
                        x2: 40,
                        y2: 75,
                        containLabel: true
                    },
                    xAxis: [
                        {
                            type: 'category',
                            data: []
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value'
                        }
                    ],

                    noDataLoadingOption: {
                        text: '暂无数据（No Data）',
                        effect: 'bubble',
                        effectOption: {
                            effect: {
                                n: 0
                            }
                        }
                    },
                    series: [
                        {
                            name: '',
                            type: 'bar',
                            center: ['50%', '30%'],
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                        },

                        {
                            name: '',
                            type: 'bar',
                            center: ['50%', '30%'],
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                        },

                        {
                            name: '',
                            type: 'bar',
                            center: ['50%', '30%'],
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                        },

                        {
                            name: '',
                            type: 'bar',
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                        },
                    ]
                };
                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);

                            for (var i = 0; i < transresult.length; i++) {

                                option1.xAxis[0].data.push(transresult[i].XName);

                                if (formType == "Column1" || formType == "Bar1") {
                                    option1.series[0].data.push(transresult[i].YNumber);
                                }
                                else if (formType == "Column2" || formType == "Bar2") {

                                    option1.series[0].data.push(transresult[i].YNumber);
                                    option1.series[1].data.push(transresult[i].ZNumber);

                                    option1.series.length = 2;

                                }
                                else if (formType == "Column3" || formType == "Bar3") {
                                    option1.series[0].data.push(transresult[i].YNumber);
                                    option1.series[1].data.push(transresult[i].ZNumber);
                                    option1.series[2].data.push(transresult[i].HNumber);

                                    option1.series.length = 3;
                                }
                                else if (formType == "Column4" || formType == "Bar4") {
                                    option1.series[0].data.push(transresult[i].YNumber);
                                    option1.series[1].data.push(transresult[i].ZNumber);
                                    option1.series[2].data.push(transresult[i].HNumber);
                                    option1.series[3].data.push(transresult[i].KNumber);

                                    option1.series.length = 4;
                                }
                                else {
                                    option1.series[0].data.push(transresult[i].YNumber);

                                    option1.series.length = 1;
                                }
                            }

                            myChart1.setOption(option1);
                        }
                    },
                    error: function (errorMsg) {
                        //alert("request data failed!!!");
                    }
                });
            }

            //横向柱状图
            if (chartType == 'Bar') {

                var option1 = {
                    title: {
                        text: chartName,
                        subtext: '',
                        itemGap: 8,
                        x: 'center',
                        y: 'center',
                        textStyle: {
                            color: '#000000',
                            fontSize: 10,
                        },
                        subtextStyle: {
                            color: '#000000',
                            fontSize: 8
                        },
                    },
                    tooltip: {
                        trigger: 'axis',
                        transitionDuration: 8,
                        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    //toolbox: {
                    //    show: false,
                    //    feature: {
                    //        saveAsImage: {
                    //            show: true
                    //        }
                    //    }
                    //},
                    //legend: {
                    //    data: ['data'],
                    //    right: '5%'
                    //},
                    grid: {
                        x: 80,
                        y: 55,
                        x2: 40,
                        y2: 80,


                        containLabel: true
                    },
                    xAxis: [
                        {
                            type: 'value',


                        }
                    ],
                    yAxis: [
                        {
                            type: 'category',
                            data: []
                        },

                        {
                            type: 'category',
                            data: []
                        }
                    ],

                    noDataLoadingOption: {
                        text: '暂无数据（No Data）',
                        effect: 'bubble',
                        effectOption: {
                            effect: {
                                n: 0
                            }
                        }
                    },
                    series: [
                        {
                            name: '',
                            type: 'bar',
                            center: ['50%', '30%'],
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                        },

                        {
                            name: '',
                            type: 'bar',
                            center: ['50%', '30%'],
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                        },

                        {
                            name: '',
                            type: 'bar',
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                        },

                        {
                            name: '',
                            type: 'bar',
                            center: ['50%', '30%'],
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                        },

                        {
                            name: '',
                            type: 'bar',
                            center: ['50%', '30%'],
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                        },
                    ]
                };
                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);

                            for (var i = 0; i < transresult.length; i++) {

                                option1.yAxis[0].data.push(transresult[i].XName);

                                if (formType == "Column1" || formType == "Bar1") {
                                    option1.series[0].data.push(transresult[i].YNumber);
                                }
                                else if (formType == "Column2" || formType == "Bar2") {

                                    option1.series[0].data.push(transresult[i].YNumber);
                                    option1.series[1].data.push(transresult[i].ZNumber);

                                    option1.series.length = 2;
                                }
                                else if (formType == "Column3" || formType == "Bar3") {

                                    option1.series[0].data.push(transresult[i].YNumber);
                                    option1.series[1].data.push(transresult[i].ZNumber);
                                    option1.series[2].data.push(transresult[i].HNumber);

                                    option1.series.length = 3;
                                }
                                else if (formType == "Column4" || formType == "Bar4") {

                                    option1.series[0].data.push(transresult[i].YNumber);
                                    option1.series[1].data.push(transresult[i].ZNumber);
                                    option1.series[2].data.push(transresult[i].HNumber);
                                    option1.series[3].data.push(transresult[i].KNumber);

                                    option1.series.length = 4;
                                }
                                else {
                                    option1.series[0].data.push(transresult[i].YNumber);

                                    option1.series.length = 1;
                                }
                            }

                            myChart1.setOption(option1);
                        }
                    },
                    error: function (errorMsg) {
                        //alert("request data failed!!!");
                    }
                });
            }

            //线图
            if (chartType == 'Line') {

                var option1 = {
                    title: {
                        text: chartName,
                        subtext: '',
                        x: 'center',
                        y: 'center',
                        textStyle: {
                            color: '#000000',
                            fontSize: 10,
                        },
                        subtextStyle: {
                            color: '#000000',
                            fontSize: 8
                        },
                    },
                    tooltip: {
                        trigger: 'axis',
                        transitionDuration: 8,
                        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    //toolbox: {
                    //    show: false,
                    //    feature: {
                    //        saveAsImage: {
                    //            show: true
                    //        }
                    //    }
                    //},
                    //legend: {
                    //    data: ['data'],
                    //    right: '5%'
                    //},
                    grid: {
                        x: 40,
                        y: 50,
                        x2: 40,
                        y2: 75,
                        containLabel: true
                    },
                    xAxis: [
                        {
                            type: 'category',
                            data: []
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value'
                        }
                    ],
                    series: [
                        {
                            name: '',
                            type: 'line',
                            center: ['50%', '30%'],
                            data: [],
                            markPoint: {
                                data: [
                                    { type: 'max', name: 'Max Value' },
                                    { type: 'min', name: 'Min Value' }
                                ]
                            },
                            markLine: {
                                data: [
                                    { type: 'average', name: '平均值' }
                                ]
                            }
                        },
                    ]
                };
                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);

                            for (var i = 0; i < transresult.length; i++) {

                                option1.xAxis[0].data.push(transresult[i].XName);
                                option1.series[0].data.push(transresult[i].YNumber)
                            }

                            myChart1.setOption(option1);
                        }
                    },
                    error: function (errorMsg) {
                        //alert("request data failed!!!");
                    }
                });
            }


            //漏斗图
            if (chartType == 'Funnel') {
                var option1 = {
                    title: {
                        text: chartName,
                        subtext: '',
                        itemGap: 8,
                        x: 'center',
                        y: 'center',
                        textStyle: {
                            color: '#000000',
                            fontSize: 10,
                        },
                        subtextStyle: {
                            color: '#000000',
                            fontSize: 8
                        },
                    },
                    tooltip: {
                        trigger: 'item',
                        transitionDuration: 8,
                        formatter: '{a} <br/>{b} : {c}%'
                    },
                    //toolbox: {
                    //    feature: {
                    //        dataView: { readOnly: false },
                    //        restore: {},
                    //        saveAsImage: {}
                    //    }
                    //},
                    legend: {
                        data: []
                    },
                    noDataLoadingOption: {
                        text: '暂无数据（No Data）',
                        effect: 'bubble',
                        effectOption: {
                            effect: {
                                n: 0
                            }
                        }
                    },
                    series: [
                        {
                            name: '',
                            type: 'funnel',
                            radius: '90%',
                            center: ['50%', '30%'],
                            top: 1,
                            bottom: 1,
                            height: 140,
                            itemStyle: {
                                borderColor: '#fff',
                                borderWidth: 1
                            },
                            //emphasis: {
                            //    label: {
                            //        fontSize: 20
                            //    }
                            //},
                            data: [

                            ]
                        }
                    ]
                };
                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);

                            var eLegend = new Array();
                            var eSeries = new Array();

                            for (var i = 0; i < transresult.length; i++) {

                                /*   eLegend.push(transresult[i].XName);*/
                                eSeries.push({
                                    value: transresult[i].YNumber,
                                    name: transresult[i].XName
                                });

                            }

                            /*   option1.legend.data = eLegend;*/
                            option1.series[0].data = eSeries;

                            myChart1.setOption(option1);
                        }
                    },
                    error: function (errorMsg) {
                        //alert("request data failed!!!");
                    }
                });
            }

            if (chartType == 'HRuningProjectStatus') {

                document.getElementById('m2').innerHTML = "<div class='card-container' style='padding-top:12px;'><div class='card blue' > <table><tr><td colpan='3' width='30%' align='center' style='padding-right:20px;'><img src = 'ImagesSkin/Running.png' alt = 'Clock Icon'/> </td><td align='left'>" + '<%=LanguageHandle.GetWord("ZaiZiXingXiangMuZhongShu").ToString() %>'.trim(); + ":<span id='spanXNumber'></span></h3> <p> " + '<%=LanguageHandle.GetWord("NianDuXingZeng").ToString() %>'.trim() + ": <span id='spanYNumber'></span></p> <p>" + '<%=LanguageHandle.GetWord("NianDuXingZeng").ToString() %>'.trim() + ":<span id='spanZNumber'></span></p></td></tr></table> </div> </div>";


                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);

                            /*  alert(result);*/

                            for (var i = 0; i < transresult.length; i++) {

                                /*   eLegend.push(transresult[i].XName);*/
                                document.getElementById("spanXNumber").innerHTML = transresult[i].XName;


                                let str = transresult[i].YNumber;
                                let parts = str.split(',');

                                document.getElementById("spanYNumber").innerHTML = parts[0];
                                document.getElementById("spanZNumber").innerHTML = parts[1];

                            }
                        }
                    },
                    error: function (errorMsg) {
                        /*  alert("Error");*/
                    }
                });
            }

            if (chartType == 'HDelayProjectStatus') {

                document.getElementById('m2').innerHTML = "<div class='card-container' style='padding-top:12px;'><div class='card red' > <table><tr><td colpan='3' width='30%' align='center' style='padding-right:20px;'><img src = 'ImagesSkin/Process.png' alt = 'Clock Icon'/> </td><td align='left'>" + '<%=LanguageHandle.GetWord("NianDuYanWuXiangMuShu").ToString() %>'.trim(); + ":<span id='spanXNumber'></span></h3> <p>" + '<%=LanguageHandle.GetWord("JingDuZhengChang").ToString() %>'.trim(); + ": <span id='spanYNumber'></span></p> <p>" + '<%=LanguageHandle.GetWord("QingDuYanWu").ToString() %>'.trim() + ": <span id='spanZNumber'></span></p></td></tr></table> </div> </div>";


                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);

                            /*  alert(result);*/

                            for (var i = 0; i < transresult.length; i++) {

                                /*   eLegend.push(transresult[i].XName);*/
                                document.getElementById("spanXNumber").innerHTML = transresult[i].XName;


                                let str = transresult[i].YNumber;
                                let parts = str.split(',');

                                document.getElementById("spanYNumber").innerHTML = parts[0];
                                document.getElementById("spanZNumber").innerHTML = parts[1];

                            }
                        }
                    },
                    error: function (errorMsg) {
                        /* alert("Error");*/
                    }
                });
            }

            if (chartType == 'HAnnualPaymentStatus') {

                document.getElementById('m2').innerHTML = "<div class='card-container' style='padding-top:12px;'><div class='card green' > <table><tr><td colpan='3' width='30%' align='center' style='padding-right:20px;'><img src = 'ImagesSkin/PaymentCollection.png' alt = 'Clock Icon'/> </td><td align='left'>" + '<%=LanguageHandle.GetWord("NianDuYanWuXiangMuShu").ToString() %>'.trim() + ":<span id='spanXNumber'></span></h3> <p>" + '<%=LanguageHandle.GetWord("NianDuChengBenHeShuan").ToString() %>'.trim() + ": <span id='spanYNumber'></span></p> <p>" + '<%=LanguageHandle.GetWord("ChengBenChaoZiXiangMuShu").ToString() %>'.trim() + ": <span id='spanZNumber'></span></p></td></tr></table> </div> </div>";


                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);


                            for (var i = 0; i < transresult.length; i++) {

                                /*   eLegend.push(transresult[i].XName);*/
                                document.getElementById("spanXNumber").innerHTML = transresult[i].XName;


                                let str = transresult[i].YNumber;
                                let parts = str.split(',');

                                document.getElementById("spanYNumber").innerHTML = parts[0];
                                document.getElementById("spanZNumber").innerHTML = parts[1];

                            }
                        }
                    },
                    error: function (errorMsg) {
                        /*alert("Error");*/
                    }
                });
            }

            if (chartType == 'HAnnualWorkHourStatus') {

                document.getElementById('m2').innerHTML = "<div class='card-container' style='padding-top:12px;'><div class='card brown' > <table><tr><td colpan='3' width='30%' align='center' style='padding-right:20px;'><img src = 'ImagesSkin/WorkHour.png' alt = 'Clock Icon'/> </td><td align='left'>" + '<%=LanguageHandle.GetWord("NianDuXiangMuGongShiTouRu").ToString() %>'.trim() + ": <span id='spanXNumber'></span></h3> <p>" + '<%=LanguageHandle.GetWord("NianDuTeiBaoRenShu").ToString() %>'.trim() + ": <span id='spanYNumber'></span></p> <p>" + '<%=LanguageHandle.GetWord("RenGongChengBen").ToString() %>'.trim() + ": <span id='spanZNumber'></span></p></td></tr></table> </div> </div>";


                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);


                            for (var i = 0; i < transresult.length; i++) {

                                /*   eLegend.push(transresult[i].XName);*/
                                document.getElementById("spanXNumber").innerHTML = transresult[i].XName;


                                let str = transresult[i].YNumber;
                                let parts = str.split(',');

                                document.getElementById("spanYNumber").innerHTML = parts[0];
                                document.getElementById("spanZNumber").innerHTML = parts[1];

                            }
                        }
                    },
                    error: function (errorMsg) {
                        /* alert("Error");*/
                    }
                });
            }

            if (chartType == 'HRuningTaskStatus') {

                document.getElementById('m2').innerHTML = "<div class='card-container' style='padding-top:12px;'><div class='card lightblue' > <table><tr><td colpan='3' width='30%' align='center' style='padding-right:20px;'><img src = 'ImagesSkin/RunningTask.png' alt = 'Clock Icon'/> </td><td align='left'>" + '<%=LanguageHandle.GetWord("ZaiZhiXingRenWuZhongShu").ToString() %>'.trim() + ": <span id='spanXNumber'></span></h3> <p>" + '<%=LanguageHandle.GetWord("NianDuXingZeng").ToString() %>'.trim() + ": <span id='spanYNumber'></span></p> <p>" + '<%=LanguageHandle.GetWord("NianDuWanCheng").ToString() %>'.trim() + ": <span id='spanZNumber'></span></p></td></tr></table> </div> </div>";


                $.ajax({
                    type: "post",
                    async: false,
                    url: "Handler/EchartHandler.ashx",
                    data: { FormType: formType, ChartName: chartName, SqlCode: sqlCode }, //发送到服务器的参数
                    datatype: "json",
                    success: function (result) {
                        if (result) {
                            eval("var transresult=" + result);

                            /*  alert(result);*/

                            for (var i = 0; i < transresult.length; i++) {

                                /*   eLegend.push(transresult[i].XName);*/
                                document.getElementById("spanXNumber").innerHTML = transresult[i].XName;


                                let str = transresult[i].YNumber;
                                let parts = str.split(',');

                                document.getElementById("spanYNumber").innerHTML = parts[0];
                                document.getElementById("spanZNumber").innerHTML = parts[1];

                            }
                        }
                    },
                    error: function (errorMsg) {
                        /* alert("Error");*/
                    }
                });
            }

            //


        });  //end page ready

    </script>

</body>
</html>
