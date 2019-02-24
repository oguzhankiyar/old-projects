<%@ page
	language="java"
	contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"
    import="java.util.*, musicbox.helpers.*, musicbox.models.*"
%>
<%
	List<Album> albums = AlbumHelper.GetTopAlbums();
	List<Song> songs = SongHelper.GetTopSongs();
%>
<!DOCTYPE html>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
		<title>Home - MusicBox</title>
		<%@ include file="head-include.jsp" %>
	</head>
	<body>
		<div style="padding: 25px 50px;">
			<jsp:include page="navbar-include.jsp"></jsp:include>
			<div class="col-xs-6">
				<div class="list-group">
					<a class="list-group-item active">Top Albums</a>
						<% for (int i = 0;i < albums.size();i++) { %>
							<a href="album.jsp?id=<%= albums.get(i).Id %>" class="list-group-item">
	  							<span style="display: inline-table; width: 30px; font-weight: bold;"><%= i + 1 %></span>
	  							<span><%= albums.get(i).Name %></span>
  							</a>
						<% } %>
				</div>
			</div>
			<div class="col-xs-6">
				<div class="list-group">
					<a class="list-group-item active">Top Songs</a>
						<% for (int i = 0;i < songs.size();i++) { %>
	  						<a href="song.jsp?id=<%= songs.get(i).Id %>" class="list-group-item">
	  							<span style="display: inline-table; width: 30px; font-weight: bold;"><%= i + 1 %></span>
	  							<span><%= songs.get(i).Name %></span>
	  						</a>
						<% } %>
				</div>
			</div>
		</div>
	</body>
</html>