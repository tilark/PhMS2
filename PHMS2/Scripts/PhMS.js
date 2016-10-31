var $PHMS = jQuery.noConflict();

$PHMS(document).ready(function () {
   
    var jsonData = {};
    //jsonData = window.performance.toJSON();
    var ajaxload = $PHMS("<img class=\"ajax-loader\" src=\"/Images/ajax-loader.gif\" />");

    //$PHMS(".changeTime").on("change", showGetResult);
    $PHMS('.changeTime').change(showGetResult);
    function showGetResult(event) {
        jsonData.startTime = $PHMS("#starttimeID").val();
        jsonData.endTime = $PHMS("#endtimeID").val();
        if (jsonData.startTime.length > 0 && jsonData.endTime.length > 0) {
            if (EndTimeBiggerStartTime(jsonData.startTime, jsonData.endTime)) {
                $PHMS(".getResult").show();
                $PHMS("#compareTime").hide();
            }
            else {
                $PHMS(".getResult").hide();
                $PHMS("#compareTime").show();

            }
        }
    }
    function EndTimeBiggerStartTime(startTimeIn, endTimeIn) {
        var startTime = new Date();
        var endTime = new Date();
        startTime.setTime(Date.parse(startTimeIn));
        endTime.setTime(Date.parse(endTimeIn));
        if (endTime.getTime() - startTime.getTime() >= 0) {
            return true;
        }
        else {
            return false;
        }
    }
    //门诊报表信息
    //$PHMS("#getOutPatientRate").click(function (event) {
    //    event.preventDefault();
    //    $PHMS(this).hide();
    //    ajaxload.insertAfter(this);

    //    $PHMS("#getOutPatientRateBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
    //        if (statusTxt == "success") {
    //        }
    //        if (statusTxt == "error") {
    //            alert("Error: " + xhr.status + ": " + xhr.statusText);
    //        }
    //        $PHMS(this).show();
    //        ajaxload.remove();

    //    });
    //});
    

    //住院报表信息
    //住院患者抗菌药物使用率（分院科两级指标）s
   
    $PHMS(".patientMessage").on("click", "input", function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        console.log($PHMS(this).attr("src"))
        var panelbody = $PHMS(this).parentsUntil(".patientMessage");
        panelbody.find(".panel-body").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();
        });
    });

    $PHMS("#getMessage").on("click", function () {
        event.preventDefault();
        ajaxload.insertAfter(this);
        $PHMS(this).hide();
    });

    $PHMS("#testBtn").on("click", function () {
        alert("InJqueryNameSpace");

    });
});

