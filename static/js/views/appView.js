'use strict';

window.App = window.App || {};

App.AppView = Backbone.View.extend({
    el: 'body', 
    initialize: function(options) {
        console.log('AppView.initialize');

        if (!options || 
            !options.eventsCollection ||
            !options.filtersCollection) {
                throw new Error("options params are missing.");
            }
        this.eventsCollection = options.eventsCollection;
        this.filtersCollection = options.filtersCollection;
        
        this.template = _.template($('.js-app-template').html());

        //Click listeners
        //this.listenTo(this.eventsCollection, 'add', this.renderFullCalendar); //TODO: check if needed
        this.listenTo(this.eventsCollection, 'sync', this.renderFullCalendar);
        this.listenTo(this.filtersCollection, 'sync', this.renderFilters);
        this.eventsCollection.fetch({
            data:{}
        });

        this.filtersCollection.fetch({
            data:{}
        });

        this.render();
    },
    render: function() {
        console.log('AppView.render');
        this.$el.append(this.template);

        this.fullCalendarView = new App.FullCalendarView({
            el: '.js-full-calendar'
        });
        this.filtersView = new App.FiltersView({
            el: '.js-filters'
        });        
    },
    renderFilters: function() {
        console.log('AppView.renderFilters');
        this.filtersView.render({
            collection: this.filtersCollection
        });
    },
    renderFullCalendar: function() {
        console.log('AppView.renderFullCalendar');
        this.fullCalendarView.render({
            collection: this.eventsCollection
        });
    }
});