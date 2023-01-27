grammar CodeGenie;

/* Class Definitions */
componentDefinition    : component (component)*;

/* Component Definition */
component           :   access_scope            // The access_scope of this component
                        NAME                    // The name of the class/interface
                        DIVIDER                 // Splitter
                        component_type          // The type of component this is
                        component_details?;     // Additional details for this component
component_type      :   INTERFACE|CLASS;
component_details   :   OPEN_S (purpose|attributes|methods|relationships|tags)+ CLOSE_S;

/* Tags */
tags                :   'tags' OPEN_S tag (tag)* CLOSE_S;
tag                 :   STRING;

/* Purpose Definition */
purpose             :   'purpose' DIVIDER value;

/* Attribute Definition */
attributes          :   'attributes' OPEN_S attribute (attribute)* CLOSE_S;
attribute           :   access_scope NAME DIVIDER type attribute_details?;
attribute_details   :   OPEN_S (purpose|tags)? CLOSE_S;

/* Method Definition */
methods             :   'methods' OPEN_S method (method)* CLOSE_S;
method              :   access_scope 
                        NAME '(' (parameter (LIST_DIVIDER parameter)*)?')' DIVIDER type
                        method_details?;
parameter           :   NAME DIVIDER type;
method_details      :   OPEN_S (purpose|tags)? CLOSE_S;

/* Relationship Definitions */
relationships       :   'relationships' OPEN_S relationship (relationship)* CLOSE_S;
relationship        :   dependency|composes|aggregates|realizes|specializes;

cardinality         :   'cardinality' DIVIDER cardinality_count '...' cardinality_count;
cardinality_count   :   '*' | NUMBER?;

dependency          :   'depends' type dependency_details?;
dependency_details  :   OPEN_S (purpose|tags)+ CLOSE_S;

composes            :   'composes' type composes_details?;
composes_details    :   OPEN_S (purpose|cardinality|tags)+ CLOSE_S;

aggregates          :   'aggregates' type aggregates_details?;
aggregates_details  :   OPEN_S (purpose|cardinality|tags)+ CLOSE_S;

realizes            :   'realizes' type realizes_details?;
realizes_details    :   OPEN_S (purpose|tags)+ CLOSE_S;

specializes         :   'specializes' type specializes_details?;
specializes_details :   OPEN_S (purpose|tags)+ CLOSE_S;

/* General Tokens */
access_scope        :   PUBLIC |            // Public
                        PRIVATE |           // Private
                        PROTECTED;          // Protected

value               :   STRING;

type                :   NAME ('<' type (',' type)* '>')? '[]'?;

PUBLIC              :   '+'|'public';
PRIVATE             :   '-'|'private';
PROTECTED           :   '#'|'protected';
OPEN_S              :   '{';
CLOSE_S             :   '}';
DIVIDER             :   ':';
LIST_DIVIDER        :   ',';
INTERFACE           :   'interface';
CLASS               :   'class';
NAME                :   [a-zA-Z][a-zA-Z1-9]*;
NUMBER              :   [1-9];

STRING              :   '"' (ESC | SAFECODEPOINT)* '"';


fragment ESC        : '\\' (["\\/bfnrt] | UNICODE);
fragment UNICODE    : 'u' HEX HEX HEX HEX;
fragment HEX        : [0-9a-fA-F] ;
fragment SAFECODEPOINT : ~["\\\u0000-\u001F];

WS                  : [ \t\n\r] + -> skip;