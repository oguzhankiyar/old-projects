<%@ page
	language="java"
	contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"
%>
    
<nav class="navbar navbar-light bg-faded">
  <a class="navbar-brand" href="index.jsp">MusicBox</a>
  <form class="form-inline navbar-form pull-right" action="search.jsp" method="post">
    <input name="query" class="form-control" type="text" placeholder="Search" autocomplete="off"
    <% if (request.getParameter("query") != null && !request.getParameter("query").trim().equals("")) { %>
    	value="<%= request.getParameter("query") %>"
    <% } %>
    />
    <button class="btn btn-success-outline" type="submit">Search</button>
  </form>
</nav>