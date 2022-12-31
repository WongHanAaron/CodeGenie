grammar CodeGenie;

/* Component Definition */
component       :   scope               // The scope of this component
                    WHITESPACE +        // One or more whitespace after
                    component_type      // The type of component this is
                    WHITESPACE+         // One or more whitespace after
                    NAME                // The name of the class/interface
                    WHITESPACE*         // An optional whitespace after
                    NEWLINE*            // An optional newline after
                    component_details?; // Additional details for this component

component_type  :   INTERFACE|CLASS;

component_details : '{' (WHITESPACE|NEWLINE)* '}';

/* Purpose Definition */
purpose         :   'purpose';

scope           :   PUBLIC |            // Public
                    PRIVATE |           // Private
                    PROTECTED;          // Protected

/* General Tokens */

PUBLIC          :   '+'|'public';
PRIVATE         :   '-'|'private';
PROTECTED       :   '#'|'protected';
INTERFACE       :   'interface';
CLASS           :   'class';
NAME            :   [a-zA-Z][a-zA-Z1-9]*;
NEWLINE         :   '\r'? '\n';
WHITESPACE      :   ' ';