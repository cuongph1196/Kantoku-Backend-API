(function () {
    'use strict';
    var app = angular.module('accnd', []);
    app.controller("printVoucherCtrl", printVoucherCtrl);
    printVoucherCtrl.$inject = ['$scope', '$http', '$window', '$timeout'];
    function printVoucherCtrl($scope, $http, $window, $timeout) {
        $scope.loading = false;
        //begin phiếu
        $scope.preview = 0;
        //var preview = parseInt(GetURLParameter('preview') ? GetURLParameter('preview') : 0);
        //var rowKey = parseInt(GetURLParameter('rowKey') ? GetURLParameter('rowKey') : 0);
        $scope.preview = preview;
        $scope.rowKey = rowKey;

        $scope.BuildingTaxName = @Model.PrintV.BuildingTaxName;

        $scope.loading = true;


        $scope.onPrintInit = function () {
            $scope.getVoucherDetails();
            if ($scope.preview === 0) {
                $timeout($window.print, 500);
            }
        };
    }
})();