angular.module("umbraco").controller("Umbraco.Pixabay.Controller", function ($scope, $http, editorState, dialogService, mediaHelper, userService, notificationsService) { 
    // Umbraco.Pixabay Controller

    $scope.notificationsService = notificationsService;
    $scope.userService = userService;
    $scope.mediaHelper = mediaHelper;
    $scope.dialogService = dialogService;

    $scope.availableOptions = null;
    $scope.advancedOptionsVisible = false;

    // Search criterias
    $scope.searchTerm = '';
    $scope.language = '';
    $scope.responseGroup = '';
    $scope.imageType = '';
    $scope.orientation = '';
    $scope.category = '';
    $scope.videoType = '';
    $scope.editorsChoice = false;
    $scope.safeSearch = false;
    $scope.minWidth = 0;
    $scope.minHeight = 0;


    $scope.data = null;
    $scope.loading = false;

    $scope.searchMode = 0;

    $scope.pageNo = 1;

    $scope.persistedSearchOptions = null;

    $scope.selectedItems = new Array();
    $scope.target = {};
    $scope.importResponse = null;

    $scope.settings = null;
    $scope.pendingSettings = null;

    $scope.displaySettings = false;
    

    $scope.loadSettings = function () {
        $.get('/umbraco/backoffice/api/pixabaysettings/getsettings', function (data) {
            $scope.settings = data;
            $scope.pendingSettings = angular.copy($scope.settings);
            $scope.displaySettings = false;
            $scope.refreshUI();
        });
    }

    $scope.setSearchMode = function(value) {
        $scope.searchMode = value;
    }

    $scope.saveSettings = function () {
        $scope.loading = true;

        $http.post('/umbraco/backoffice/api/pixabaysettings/savesettings', { settings: $scope.pendingSettings }).then(function (response) {

            $scope.loadSettings();
            $scope.loading = false;
        });
    }

    $scope.getAvailableOptions = function () {
        $.get('/umbraco/backoffice/api/pixabaysearch/GetAvailableOptions', function (data) {
            $scope.availableOptions = data;
            $scope.loading = false;
            $scope.refreshUI();
        });
    }

    $scope.showSettings = function () {

        if ($scope.displaySettings === true) {
            return $scope.displaySettings;
        }

        if ($scope.settings) {
            return ($scope.settings.apiKey === null);
        }
        else {
            return true;
        }
    }


    $scope.search = function () {
        
        $scope.pageNo = 1;
        $scope.selectedItems = new Array();
        $scope.persistedSearchOptions = $scope.getOptions();
        $scope.loadData();
    }

    $scope.loadData = function() {

        $scope.loading = true;

        if ($scope.searchMode === 0) {
            $http.post('/umbraco/backoffice/api/PixabaySearch/SearchImages', $scope.persistedSearchOptions).then(function (response) {
                $scope.data = response.data;
                $scope.loading = false;
                $scope.refreshUI();
            });
        } else {
            $http.post('/umbraco/backoffice/api/PixabaySearch/SearchVideos', $scope.persistedSearchOptions).then(function (response) {
                $scope.data = response.data;
                $scope.loading = false;
                $scope.refreshUI();
            });
        }

    }

    $scope.nextPage = function () {
        $scope.pageNo++;
        $scope.persistedSearchOptions.page = $scope.pageNo;
        $scope.loadData();
    }

    $scope.prevPage = function () {
        if ($scope.pageNo > 0) {
            $scope.pageNo--;
            $scope.persistedSearchOptions.page = $scope.pageNo;
            $scope.loadData();
        }
    }

    $scope.getOptions = function () {
        return {
            searchTerm: $scope.searchTerm,
            language: $scope.language,
            responseGroup: $scope.responseGroup,
            imageType: $scope.imageType,
            orientation: $scope.orientation,
            category: $scope.category,
            videoType: $scope.videoType,
            editorsChoice: $scope.editorsChoice,
            safeSearch: $scope.safeSearch,
            minWidth: $scope.minWidth,
            minHeight: $scope.minHeight,
            page: $scope.pageNo
        }
    }


    // Enter key in search-terms.
    $scope.searchKeyUp = function () {
        if (event !== null) {
            if (event.keyCode) {
                if (event.keyCode === 13) {
                    $scope.search();
                }
            }
        }
    }

    $scope.clickItem = function (item, evt, idx) {
        if (item !== null) {
            if (_.contains($scope.selectedItems, item)) {
                var index = $scope.selectedItems.indexOf(item);
                $scope.selectedItems.splice(index, 1);
            }
            else {
                $scope.selectedItems.push(item);
            }
        }
    }

    $scope.isAnythingSelected = function () {
        return ($scope.selectedItems !== null && $scope.selectedItems.length > 0);
    }

    $scope.clearSelection = function () {
        $scope.selectedItems = new Array();
    }

    $scope.isSelected = function(item) {
        return _.contains($scope.selectedItems, item);
    }

    $scope.toogleAdvancedOptions = function () {
        $scope.advancedOptionsVisible = !$scope.advancedOptionsVisible;
    }

    $scope.toogleSettings = function () {
        $scope.displaySettings = !$scope.displaySettings;
    }


    $scope.refreshUI = function () {
        $scope.safeApply($scope, () => { });
    }

    $scope.safeApply = function (scope, fn) {
        (scope.$$phase || scope.$root.$$phase) ? fn() : scope.$apply(fn);
    }

    $scope.transfer = function () {



        $scope.userService.getCurrentUser().then(function (userData) {

            var mediaStartNode = -1;
            if (userData.startMediaIds !== null) {
                if (Array.isArray(userData.startMediaIds)) {
                    if (userData.startMediaIds.length > 0) {
                        mediaStartNode = userData.startMediaIds[0];
                    }
                }
            }

            $scope.dialogService.mediaPicker({
                startNodeId: mediaStartNode,
                callback: function (media) {
                    $scope.target.id = media.id;
                    $scope.target.isMedia = true;
                    $scope.target.name = media.name;
                    $scope.refreshUI();
                }
            });

        });

    }

    $scope.readyForImport = function () {
        return ($scope.target.id > 0 && $scope.selectedItems !== null);
    }

    $scope.confirmedImport = function () {
        if ($scope.target.id > 0) {
            $scope.dialogService.closeAll();
            $scope.performImport($scope.target.id, $scope.selectedItems);
        }
    }

    $scope.cancelImport = function () {
        $scope.target.id = 0;
        $scope.refreshUI();
    }

    $scope.performImport = function (folderId, imageEntries) {

        $scope.target.id = 0;
        $scope.loading = true;
        var postUrl = '';
        if ($scope.searchMode === 0) {
            postUrl = '/umbraco/backoffice/api/PixabayImport/Import';
        }
        else {
            postUrl = '/umbraco/backoffice/api/PixabayImport/ImportVideo';
        }

        $http.post(postUrl, { mediafolder: folderId, imageEntries: imageEntries, mediaType: $scope.searchMode }).then(function (response) {

            $scope.importResponse = response.data;

            if (response) {
                if (response.data) {

                    if (response.data.successList) {

                        $scope.notificationsService.success("Import completed.", response.data.successList.length + " files was imported.");
                        if (response.data.failureList.length > 0) {
                            $scope.notificationsService.error(response.data.failureList.length + " file(s) not imported.", "Some files were not imported correctly.");
                        }

                    }
                    else {
                        $scope.notificationsService.error("Import completed.", "No files was successfully imported.");
                    }
                }
                else {
                    $scope.notificationsService.error("Import failed.", "Inconclusive answer from server. Check media folder to see if import succeeded.");
                }
            }
            else {
                $scope.notificationsService.error("Import failed.", "Inconclusive answer from server. Check media folder to see if import succeeded.");
            }

            $scope.clearSelection();
            $scope.loading = false;
            $scope.refreshUI();

        });


    }

   

    $scope.loadSettings();
    $scope.getAvailableOptions();
   

 
}); 
