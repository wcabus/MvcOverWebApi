(function () {

    // Let's set some default first for every AJAX call.
    $.ajaxSetup({
        xhrFields: {
            withCredentials: true // This setting passes cookies along the request, including our authentication cookie.
        }
    });
    
    function TodoItem() {
        var self = this;

        self.id = '';
        self.description = ko.observable('');
        self.done = ko.observable(false);
        self.userId = '';
    }

    function ViewModel() {
        var self = this;
        
        //ViewState
        self.showListView = ko.observable(true);
        self.showAddItem = ko.observable(true);

        //Data
        self.items = ko.observableArray([]);
        self.newItem = ko.observable(new TodoItem());

        //Functions
        self.load = function () {
            $.ajax(apiSettings.baseUrl + '/api/TodoItem', {
                dataType: 'json'
            }).done(function (data) {
                self.items.removeAll();

                for (var i = 0; i < data.length; i++) {
                    var item = new TodoItem();
                    item.id = data[i].Id;
                    item.description(data[i].Description);
                    item.done(data[i].Done);
                    item.userId = data[i].User.Id;

                    self.items.push(item);
                }
            });
        };

        self.addNewItem = function () {
            $.ajax(apiSettings.baseUrl + '/api/TodoItem', {
                type: 'POST',
                data: {
                    description: self.newItem().description()
                }
            })
            .done(function () {
                self.load();
                self.newItem(new TodoItem());
            });
        };
    }

    var vm = new ViewModel();
    ko.applyBindings(vm);

    vm.load();
})(ko);