<%@ page
	language="java"
	contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"
    import="java.util.*, musicbox.helpers.*, musicbox.models.*"
%>
<%
	String artistId = request.getParameter("id");
	Artist artist = ArtistHelper.GetArtistDetails(artistId);
%>
<!DOCTYPE html>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
		<title><%= artist.Name %> - MusicBox</title>
		<%@ include file="head-include.jsp" %>
	</head>
	<body>
		<div style="padding: 25px 50px;">
			<jsp:include page="navbar-include.jsp"></jsp:include>
			<div style="display: table; margin-left: 30%;">
				<div style="float: left;">
					<img src="<%= artist.Albums.get(0).ImageUrl %>" width="150" />
				</div>
				<div style="float: left; padding: 5px 15px; font-size: 20px;">
					<font style="font-size: 30px;"><%= artist.Name %></font><br />
					<b>Primary Genre:</b> <%= artist.PrimaryGenre %><br />
					<b>Albums: </b><%= artist.Albums.size() %><br />
					<b>Songs: </b><%= artist.Songs.size() %>
				</div>
			</div>
			<div style="display: table; width: 100%; margin-top: 20px;">
				<div class="col-xs-6">
					<div class="list-group">
						<a class="list-group-item active"><%= artist.Name.split(" ")[0] %>'s Albums</a>
							<% List<Album> albums = artist.Albums; %>
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
						<a class="list-group-item active"><%= artist.Name.split(" ")[0] %>'s Songs</a>
							<% List<Song> songs = artist.Songs; %>
							<% for (int i = 0;i < songs.size();i++) { %>
		  						<a href="song.jsp?id=<%= songs.get(i).Id %>" class="list-group-item">
		  							<span style="display: inline-table; width: 30px; font-weight: bold;"><%= i + 1 %></span>
		  							<span><%= songs.get(i).Name %></span>
		  						</a>
							<% } %>
					</div>
				</div>
			</div>
		</div>
	</body>
</html>