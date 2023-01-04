grammar CodeGenie;

/* Class Definitions */
class_definition    : component (',' component)*;

/* Component Definition */
component           :   access_scope            // The access_scope of this component
                        NAME                    // The name of the class/interface
                        DIVIDER                 // Splitter
                        component_type          // The type of component this is
                        component_details?;     // Additional details for this component
component_type      :   INTERFACE|CLASS;
component_details   :   OPEN_S (purpose|attributes|methods|relationships)+ CLOSE_S;

/* Purpose Definition */
purpose             :   'purpose' OPEN_S value CLOSE_S;

/* Attribute Definition */
attributes          :   'attributes' OPEN_S attribute (',' attribute)* CLOSE_S;
attribute           :   access_scope NAME DIVIDER type attribute_details?;
attribute_details   :   OPEN_S purpose+ CLOSE_S;

/* Method Definition */
methods             :   'methods' OPEN_S method (',' method)* CLOSE_S;
method              :   access_scope 
                        NAME '(' (parameter (',' parameter)*)?')' DIVIDER type
                        method_details?;
parameter           :   NAME DIVIDER type;
method_details      :   OPEN_S purpose+ CLOSE_S;

/* Relationship Definitions */
relationships       :   'relationships' OPEN_S relationship (',' relationship)* CLOSE_S;
relationship        : ;



/* General Tokens */
access_scope        :   PUBLIC |            // Public
                        PRIVATE |           // Private
                        PROTECTED;          // Protected

value               :   STRING;

type                :   NAME ('<' type (',' type)* '>')?;

PUBLIC              :   '+'|'public';
PRIVATE             :   '-'|'private';
PROTECTED           :   '#'|'protected';
OPEN_S              :   '{';
CLOSE_S             :   '}';
DIVIDER             :   ':';
INTERFACE           :   'interface';
CLASS               :   'class';
NAME                :   [a-zA-Z][a-zA-Z1-9]*;

STRING              :   '"' (ESC | SAFECODEPOINT)* '"';


fragment ESC        : '\\' (["\\/bfnrt] | UNICODE);
fragment UNICODE    : 'u' HEX HEX HEX HEX;
fragment HEX        : [0-9a-fA-F] ;
fragment SAFECODEPOINT : ~["\\\u0000-\u001F];

WS                  : [ \t\n\r] + -> skip;