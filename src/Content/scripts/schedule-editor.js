(function(){
  var Movie = Backbone.Model.extend({
    initialize: function(){
      console.log("we got model");
    }
  });

  var MovieCollection = new Backbone.Collection;
  MovieCollection.model = Movie;
  MovieCollection.url = '../movies/list';
  MovieCollection.fetch();

  var MovieView = Backbone.View.extend({
    tagName: "li",
    
    template: _.template($('#movie-item-template').html()),
    
    render: function() {
      $(this.el).html(this.template(this.model.toJSON()));
      return this;
    }    
  });

  var EditorView = Backbone.View.extend({
    el: $('#movie-list-area'),

    render: function( event ){
      return this;
    },
    initialize: function(){
        MovieCollection.bind('add', this.addOne, this);     
        MovieCollection.bind('all', this.render, this);     
        MovieCollection.bind('reset', this.addAll, this);     
    },
    addAll: function(){
      MovieCollection.each(this.addOne);
    },
    addOne: function(movie){
      var view = new MovieView({model: movie});
      $("#movie-list").append(view.render().el); 
    },
    events: {
    }
  });

  var myApp = new EditorView;
}());
