#TB(name="header", NL="\r\n")
POST /post_json HTTP/1.1
Content-Type: application/json
Auth-Token: 123456

#TE("\r\n\r\n")
#TB(name="body", NL="\r\n")
{
	"id": 123,
	"data": [1, 2, 3, 4]
}
#TE(EOF)
