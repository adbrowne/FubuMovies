(function(){
  var MovieSession = Backbone.Model.extend({
    initialize: function(){
      console.log("movie session");
    }
  })

  var Movie = Backbone.Model.extend({
    initialize: function(){
      console.log("we got model");
    }
  });

  var MovieSessionCollection = new Backbone.Collection;
  MovieSessionCollection.model = MovieSession;
  MovieSessionCollection.url = '../api/moviesessions';
  MovieSessionCollection.fetch();

  var MovieCollection = new Backbone.Collection;
  MovieCollection.model = Movie;
  MovieCollection.url = '../api/movies';
  MovieCollection.fetch();

  var MovieSessionView = Backbone.View.extend({
    tagName: "li",
    
    template: _.template($('#moviesession-item-template').html()),
    
    render: function() {
      $(this.el).html(this.template(this.model.toJSON()));
      return this;
    }    
  });

  var NewSessionView = Backbone.View.extend({
    template: _.template($('#moviesession-new-template').html()),

    tagName: "div",
    
    saveSession: function(e){
      console.log("saveSession");
    },

    render: function() {
      var view = this;
      $(this.el).html(this.template());
      $(this.el).addClass("modal"); 
      $(this.el).addClass("hide"); 
      $(this.el).addClass("fade"); 
      this.startTime = $(this.el).find("#new-session-time");
      $(this.el).on("click", ".btn-primary", function(e){

	$(view.el).modal('hide');
	MovieSessionCollection.create(
        {
	  Entity: {
            StartTime: view.startTime.val(),
            Movie: view.model.Entity.Movie
	  }
	});
	console.log("inline saveSession");		   
      });
      return this;
    },    

    events: {
      "click .btn-primary": "saveSession"
    }

  });

  var MovieView = Backbone.View.extend({
    tagName: "li",
    
    template: _.template($('#movie-item-template').html()),

    events: {
      "click .add-session": "addSession"
    },
    
    addSession: function(e){
      var movieId = this.model.get("Id");

      var newSessionView = new NewSessionView({
	model: {
	  Entity: {
	    Movie: this.model.get("Entity")
	  }
	}
      });

      var el = newSessionView.render().el;
      $(el).modal();
      console.log("Movie id: " + movieId);
      console.log("add session");
      console.log(e);
    }, 
    
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
      MovieCollection.bind('add', this.addOneMovie, this);     
      MovieCollection.bind('all', this.render, this);     
      MovieCollection.bind('reset', this.addAllMovies, this);

      MovieSessionCollection.bind('add', this.addOneSession, this);     
      MovieSessionCollection.bind('all', this.render, this);     
      MovieSessionCollection.bind('reset', this.addAllSessions, this);
    },
    addAllSessions: function(){
      MovieSessionCollection.each(this.addOneSession);
    },
    addOneSession: function(movieSession){
      var view = new MovieSessionView({model: movieSession});
      $("#movie-session-list").append(view.render().el); 
    },
    addAllMovies: function(){
      MovieCollection.each(this.addOneMovie);
    },
    addOneMovie: function(movie){
      var view = new MovieView({model: movie});
      $("#movie-list").append(view.render().el); 
    },
    events: {
      "click #movie-add": "addMovie"
    },
    addMovie: function(e){
      MovieCollection.create(
        {
	  Entity: {
	    Name: this.new_name.val(),
	    Description: this.new_description.val()
	  }
        });
      console.log("adding movie");
    }
  });

  var myApp = new EditorView;
}());
