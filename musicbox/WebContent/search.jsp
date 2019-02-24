<%@ page
	language="java"
	contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"
    import="java.util.*, musicbox.helpers.*, musicbox.models.*"
%>
<%
	String query = request.getParameter("query");
	List<BaseObject> results = SearchHelper.Search(query.replace(" ", "-"));
%>
<!DOCTYPE html>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
		<title><%= query %> - MusicBox</title>
		<%@ include file="head-include.jsp" %>
	</head>
	<body>
		<div style="padding: 25px 50px;">
			<jsp:include page="navbar-include.jsp"></jsp:include>
			<div style="display: table; width: 100%; margin-top: 20px;">
				<div>
					<div class="list-group">
						<a class="list-group-item active">Results for '<%= query %>'</a>
							<% for (int i = 0;i < results.size();i++) {
								BaseObject result = results.get(i); %>
								<% if (result.getClass() == Song.class) {
									Song song = (Song)result; %>
			  						<a href="song.jsp?id=<%= song.Id %>" class="list-group-item">
			  							<span style="display: inline-table; width: 70px; opacity: .5">Song</span>
			  							<span><%= song.Name %></span>
			  						</a>
			  					<% } %>
								<% if (result.getClass() == Album.class) {
									Album album = (Album)result; %>
			  						<a href="album.jsp?id=<%= album.Id %>" class="list-group-item">
			  							<span style="display: inline-table; width: 70px; opacity: .5">Album</span>
			  							<span><%= album.Name %></span>
			  						</a>
			  					<% } %>
								<% if (result.getClass() == Artist.class) {
									Artist artist = (Artist)result; %>
			  						<a href="artist.jsp?id=<%= artist.Id %>" class="list-group-item">
			  							<span style="display: inline-table; width: 70px; opacity: .5">Artist</span>
			  							<span><%= artist.Name %></span>
			  						</a>
			  					<% } %>
							<% } %>
					</div>
				</div>
			</div>
		</div>
	</body>
</html>