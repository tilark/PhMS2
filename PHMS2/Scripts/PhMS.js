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
    $PHMS("#getOutPatientRate").click(function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);

        $PHMS("#getOutPatientRateBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();

        });
    });



    $PHMS("#getEmergyRate").click(function (event) {
        event.preventDefault();

        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        $PHMS("#getEmergyRateBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();

        });
    });

    $PHMS("#getAverageCost").click(function (event) {
        event.preventDefault();

        $PHMS(this).hide();
        ajaxload.insertAfter(this);

        $PHMS("#getAverageCostBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();

        });
    });
    $PHMS("#getTopTenAntibiotic").click(function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        $PHMS("#getTopTenAntibioticBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();
        });
    });
    $PHMS("#getTopTenAntibioticDep").click(function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        $PHMS("#getTopTenAntibioticDepBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();
        });
    });
    $PHMS("#getTopThirtyDrug").click(function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        $PHMS("#getTopThirtyDrugBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();
        });
    });
    $PHMS("#getOutPatientDrugDetails").click(function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        $PHMS("#getOutPatientDrugDetailsBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();
        });
    });
    $PHMS("#getEssentialDrugRate").click(function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        $PHMS("#getEssentialDrugRateBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();
        });
    });
    $PHMS("#getAverageDrugCategory").click(function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        $PHMS("#getAverageDrugCategoryBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();
        });
    });

    $PHMS("#getOutPatientVeinInfusionRate").click(function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        $PHMS("#getOutPatientVeinInfusionRateBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
            }
            if (statusTxt == "error") {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
            $PHMS(this).show();
            ajaxload.remove();
        });
    });
    $PHMS("#getEmergyVeinInfusionRate").click(function (event) {
        event.preventDefault();
        $PHMS(this).hide();
        ajaxload.insertAfter(this);
        $PHMS("#getEmergyVeinInfusionRateBody").load($PHMS(this).attr("src"), jsonData, function (responseTxt, statusTxt, xhr) {
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

