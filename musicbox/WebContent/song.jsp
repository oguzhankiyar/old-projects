<%@ page
	language="java"
	contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"
    import="java.util.*, musicbox.helpers.*, musicbox.models.*"
%>
<%
	String songId = request.getParameter("id");
	Song song = SongHelper.GetSongDetails(songId);
%>
<!DOCTYPE html>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
		<title><%= song.Name %> - MusicBox</title>
		<%@ include file="head-include.jsp" %>
	</head>
	<body>
		<div style="padding: 25px 50px;">
			<jsp:include page="navbar-include.jsp"></jsp:include>
			<div style="display: table; margin-left: 30%;">
				<div style="float: left;">
					<img src="<%= song.ImageUrl %>" width="150" />
				</div>
				<div style="float: left; padding: 5px 15px; font-size: 20px;">
					<font style="font-size: 30px;"><%= song.Name %></font><br />
					<b>Artist: </b><a href="artist.jsp?id=<%= song.Artist.Id %>"><%= song.Artist.Name %></a><br />
					<b>Album: </b><a href="album.jsp?id=<%= song.Album.Id %>"><%= song.Album.Name %></a><br />
		            <audio id="player" controls="controls" volume=".5" style="margin-top: 5px;">
		            	<source src="<%= song.PreviewUrl %>" type="audio/ogg">
	  					<source src="<%= song.PreviewUrl %>" type="audio/mpeg">
		            </audio>
				</div>
			</div>
		</div>
	</body>
</html>