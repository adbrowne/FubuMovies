(function(){
  var Movie = Backbone.Model.extend({
    initialize: function(){
      console.log("we got model");
    }
  });

  var MovieCollection = new Backbone.Collection;
  MovieCollection.model = Movie;
  MovieCollection.url = '../api/movies';
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
    el: $('#editor'),

    render: function( event ){
      return this;
    },
    initialize: function(){
      this.new_name = this.$("#new-name");
      this.new_description = this.$("#new-description");
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
      "click #movie-add": "addMovie"
    },
    addMovie: function(e){
      MovieCollection.create(
        {
          Name: this.new_name.val(),
          Description: this.new_description.val()
        });
      console.log("adding movie");
    }
  });

  var myApp = new EditorView;
}());
