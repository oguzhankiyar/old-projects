<%@ page
	language="java"
	contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"
    import="java.util.*, musicbox.helpers.*, musicbox.models.*"
%>
<%
	String albumId = request.getParameter("id");
	Album album = AlbumHelper.GetAlbumDetails(albumId);
%>
<!DOCTYPE html>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
		<title><%= album.Name %> - MusicBox</title>
		<%@ include file="head-include.jsp" %>
	</head>
	<body>
		<div style="padding: 25px 50px;">
			<jsp:include page="navbar-include.jsp"></jsp:include>
			<div style="display: table; margin-left: 30%;">
				<div style="float: left;">
					<img src="<%= album.ImageUrl %>" width="150" />
				</div>
				<div style="float: left; padding: 5px 15px; font-size: 20px;">
					<font style="font-size: 30px;"><%= album.Name %></font><br />
					<b>Album: </b><a href="artist.jsp?id=<%= album.Artist.Id %>"><%= album.Artist.Name %></a><br />
					<b>Songs: </b><%= album.Songs.size() %>
				</div>
			</div>
			<div style="display: table; width: 100%; margin-top: 20px;">
				<div>
					<div class="list-group">
						<a class="list-group-item active">Album Songs</a>
							<% List<Song> songs = album.Songs; %>
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